using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 
/// MADE ENTIRELY BY CLAUDE (with some instructions ans guidence)
/// 
/// Cuts a mesh in two with a plane, fills the hole on both halves and
/// creates a new GameObject for the smaller half.
/// The original GameObject keeps the bigger half.
/// </summary>
public static class MeshSlicer
{
    /// <summary>
    /// Slices "target" with the plane defined by a world point and a world normal.
    /// Returns the new GameObject (the cut piece) or null if the plane does not cross the mesh.
    /// </summary>
    public static GameObject Slice(GameObject target, Vector3 worldPoint, Vector3 worldNormal, Material capMaterial)
    {
        MeshFilter mf = target.GetComponent<MeshFilter>();
        MeshRenderer mr = target.GetComponent<MeshRenderer>();

        if (mf == null || mr == null || mf.sharedMesh == null)
        {
            Debug.LogWarning($"MeshSlicer: '{target.name}' needs a MeshFilter and a MeshRenderer.", target);
            return null;
        }

        Mesh mesh = mf.sharedMesh;

        if (!mesh.isReadable)
        {
            Debug.LogWarning($"MeshSlicer: enable Read/Write in the import settings of '{mesh.name}'.", target);
            return null;
        }

        Transform t = target.transform;

        // Plane in the mesh's local space (works with non-uniform scale)
        Vector3 localPoint = t.InverseTransformPoint(worldPoint);
        Vector3 localNormal = t.localToWorldMatrix.transpose.MultiplyVector(worldNormal).normalized;

        // ---------- Mesh data ----------
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        if (normals.Length != verts.Length)
        {
            Mesh tmp = Object.Instantiate(mesh);
            tmp.RecalculateNormals();
            normals = tmp.normals;
            Object.Destroy(tmp);
        }
        if (uvs.Length != verts.Length) uvs = new Vector2[verts.Length];

        // ---------- Materials / submeshes ----------
        Material[] srcMats = mr.sharedMaterials;
        int origSubs = mesh.subMeshCount;

        // If this object was already cut, its last submesh is the cap: reuse it instead of adding another one
        bool alreadyHasCap = capMaterial != null && srcMats.Length >= origSubs && srcMats[origSubs - 1] == capMaterial;
        int capSub = alreadyHasCap ? origSubs - 1 : origSubs;
        int totalSubs = capSub + 1;

        Material[] mats = new Material[totalSubs];
        for (int i = 0; i < totalSubs; i++)
        {
            if (i == capSub) mats[i] = capMaterial != null ? capMaterial : srcMats[0];
            else mats[i] = i < srcMats.Length ? srcMats[i] : srcMats[srcMats.Length - 1];
        }

        // ---------- Split triangles ----------
        float[] dist = new float[verts.Length];
        for (int i = 0; i < verts.Length; i++) dist[i] = Vector3.Dot(verts[i] - localPoint, localNormal);

        Side pos = new Side(totalSubs); // dist >= 0
        Side neg = new Side(totalSubs); // dist < 0
        List<Vector3> cutSegments = new List<Vector3>(); // pairs of points on the plane

        for (int s = 0; s < origSubs; s++)
        {
            if (mesh.GetTopology(s) != MeshTopology.Triangles) continue;

            int[] tris = mesh.GetTriangles(s);

            for (int k = 0; k < tris.Length; k += 3)
            {
                int a = tris[k], b = tris[k + 1], c = tris[k + 2];
                bool pa = dist[a] >= 0f, pb = dist[b] >= 0f, pc = dist[c] >= 0f;

                // Whole triangle on one side
                if (pa == pb && pb == pc)
                {
                    Side side = pa ? pos : neg;
                    side.tris[s].Add(side.AddOriginal(a, verts, normals, uvs));
                    side.tris[s].Add(side.AddOriginal(b, verts, normals, uvs));
                    side.tris[s].Add(side.AddOriginal(c, verts, normals, uvs));
                    continue;
                }

                // Find the lone vertex, keeping the winding order (lone, r1, r2)
                int l, r1, r2;
                if (pa == pb) { l = c; r1 = a; r2 = b; }
                else if (pa == pc) { l = b; r1 = c; r2 = a; }
                else { l = a; r1 = b; r2 = c; }

                Side lone = dist[l] >= 0f ? pos : neg;
                Side other = dist[l] >= 0f ? neg : pos;

                // Intersection points on edges l-r1 and l-r2
                float t1 = dist[l] / (dist[l] - dist[r1]);
                float t2 = dist[l] / (dist[l] - dist[r2]);

                Vector3 p1 = Vector3.Lerp(verts[l], verts[r1], t1);
                Vector3 p2 = Vector3.Lerp(verts[l], verts[r2], t2);
                Vector3 n1 = Vector3.Lerp(normals[l], normals[r1], t1).normalized;
                Vector3 n2 = Vector3.Lerp(normals[l], normals[r2], t2).normalized;
                Vector2 u1 = Vector2.Lerp(uvs[l], uvs[r1], t1);
                Vector2 u2 = Vector2.Lerp(uvs[l], uvs[r2], t2);

                // Lone side: one triangle (l, p1, p2)
                int L = lone.AddOriginal(l, verts, normals, uvs);
                int la = lone.AddNew(p1, n1, u1);
                int lb = lone.AddNew(p2, n2, u2);
                lone.tris[s].Add(L); lone.tris[s].Add(la); lone.tris[s].Add(lb);

                // Other side: quad (p1, r1, r2, p2) -> two triangles
                int oa = other.AddNew(p1, n1, u1);
                int R1 = other.AddOriginal(r1, verts, normals, uvs);
                int R2 = other.AddOriginal(r2, verts, normals, uvs);
                int ob = other.AddNew(p2, n2, u2);
                other.tris[s].Add(oa); other.tris[s].Add(R1); other.tris[s].Add(R2);
                other.tris[s].Add(oa); other.tris[s].Add(R2); other.tris[s].Add(ob);

                cutSegments.Add(p1);
                cutSegments.Add(p2);
            }
        }

        // Plane did not cross the mesh
        if (cutSegments.Count == 0 || pos.TriangleCount == 0 || neg.TriangleCount == 0) return null;

        // ---------- Fill the hole (cap) ----------
        Vector3 center = Vector3.zero;
        foreach (Vector3 p in cutSegments) center += p;
        center /= cutSegments.Count;

        Vector3 uAxis = Vector3.Cross(localNormal, Mathf.Abs(localNormal.y) < 0.99f ? Vector3.up : Vector3.right).normalized;
        Vector3 vAxis = Vector3.Cross(localNormal, uAxis);

        pos.AddCap(cutSegments, center, -localNormal, uAxis, vAxis, capSub); // positive half faces towards the negative side
        neg.AddCap(cutSegments, center, localNormal, uAxis, vAxis, capSub);

        // ---------- Build meshes ----------
        Mesh posMesh = pos.Build(mesh.name + "_A");
        Mesh negMesh = neg.Build(mesh.name + "_B");

        // The bigger half stays on the original object
        bool keepPos = Volume(posMesh.bounds) >= Volume(negMesh.bounds);
        Mesh keptMesh = keepPos ? posMesh : negMesh;
        Mesh pieceMesh = keepPos ? negMesh : posMesh;

        // Original object
        mf.sharedMesh = keptMesh;
        mr.sharedMaterials = mats;
        SetMeshCollider(target, keptMesh, target.GetComponent<Rigidbody>() != null);

        // New piece
        GameObject piece = new GameObject(target.name + "_Slice");
        piece.tag = target.tag;
        piece.layer = target.layer;
        piece.transform.SetParent(t.parent, false);
        piece.transform.localPosition = t.localPosition;
        piece.transform.localRotation = t.localRotation;
        piece.transform.localScale = t.localScale;

        piece.AddComponent<MeshFilter>().sharedMesh = pieceMesh;
        MeshRenderer pr = piece.AddComponent<MeshRenderer>();
        pr.sharedMaterials = mats;
        pr.shadowCastingMode = mr.shadowCastingMode;
        pr.receiveShadows = mr.receiveShadows;

        SetMeshCollider(piece, pieceMesh, true); // Convex: needed for a non-kinematic Rigidbody

        return piece;
    }

    // Replaces any other collider with a MeshCollider that matches the new mesh
    static void SetMeshCollider(GameObject go, Mesh mesh, bool convex)
    {
        foreach (Collider col in go.GetComponents<Collider>())
        {
            if (!(col is MeshCollider)) Object.Destroy(col);
        }

        MeshCollider mc = go.GetComponent<MeshCollider>();
        if (mc == null) mc = go.AddComponent<MeshCollider>();

        mc.sharedMesh = null; // Forces Unity to rebuild the collider
        mc.sharedMesh = mesh;
        mc.convex = convex;
    }

    static float Volume(Bounds b) => b.size.x * b.size.y * b.size.z;

    // ---------- Data for one half ----------
    class Side
    {
        public readonly List<Vector3> verts = new List<Vector3>();
        public readonly List<Vector3> normals = new List<Vector3>();
        public readonly List<Vector2> uvs = new List<Vector2>();
        public readonly List<int>[] tris;

        readonly Dictionary<int, int> map = new Dictionary<int, int>();

        public Side(int subCount)
        {
            tris = new List<int>[subCount];
            for (int i = 0; i < subCount; i++) tris[i] = new List<int>();
        }

        public int TriangleCount
        {
            get
            {
                int count = 0;
                foreach (List<int> list in tris) count += list.Count;
                return count / 3;
            }
        }

        public int AddOriginal(int i, Vector3[] v, Vector3[] n, Vector2[] uv)
        {
            if (map.TryGetValue(i, out int idx)) return idx;
            idx = AddNew(v[i], n[i], uv[i]);
            map[i] = idx;
            return idx;
        }

        public int AddNew(Vector3 v, Vector3 n, Vector2 uv)
        {
            verts.Add(v);
            normals.Add(n);
            uvs.Add(uv);
            return verts.Count - 1;
        }

        // Triangle fan from the center of the cut to every cut segment
        public void AddCap(List<Vector3> segments, Vector3 center, Vector3 faceNormal, Vector3 uAxis, Vector3 vAxis, int capSub)
        {
            Dictionary<Vector3, int> capVerts = new Dictionary<Vector3, int>();

            int Get(Vector3 p)
            {
                if (capVerts.TryGetValue(p, out int idx)) return idx;
                Vector3 d = p - center;
                idx = AddNew(p, faceNormal, new Vector2(Vector3.Dot(d, uAxis), Vector3.Dot(d, vAxis)) + new Vector2(0.5f, 0.5f));
                capVerts[p] = idx;
                return idx;
            }

            int c = Get(center);

            for (int i = 0; i < segments.Count; i += 2)
            {
                Vector3 a = segments[i];
                Vector3 b = segments[i + 1];

                // Skip degenerate triangles
                Vector3 cross = Vector3.Cross(a - center, b - center);
                if (cross.sqrMagnitude < 1e-12f) continue;

                int ia = Get(a);
                int ib = Get(b);

                // Unity front face: normal = Cross(v1 - v0, v2 - v0)
                if (Vector3.Dot(cross, faceNormal) >= 0f) { tris[capSub].Add(c); tris[capSub].Add(ia); tris[capSub].Add(ib); }
                else { tris[capSub].Add(c); tris[capSub].Add(ib); tris[capSub].Add(ia); }
            }
        }

        public Mesh Build(string name)
        {
            Mesh m = new Mesh { name = name };
            if (verts.Count > 65535) m.indexFormat = IndexFormat.UInt32;

            m.SetVertices(verts);
            m.SetNormals(normals);
            m.SetUVs(0, uvs);

            m.subMeshCount = tris.Length;
            for (int i = 0; i < tris.Length; i++) m.SetTriangles(tris[i], i);

            m.RecalculateBounds();
            m.RecalculateTangents();
            return m;
        }
    }
}
