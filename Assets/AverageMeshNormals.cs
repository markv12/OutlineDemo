using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AverageMeshNormals : MonoBehaviour {
    public MeshFilter[] meshSources;
    private static readonly Vector3 zeroVec = Vector3.zero;
    // Use this for initialization
    void Start() {
        foreach (MeshFilter meshSource in meshSources) {
            Vector3[] verts = meshSource.mesh.vertices;
            Vector3[] normals = meshSource.mesh.normals;
            VertInfo[] vertInfo = new VertInfo[verts.Length];
            for (int i = 0; i < verts.Length; i++) {
                vertInfo[i] = new VertInfo() {
                    vert = verts[i],
                    origIndex = i,
                    normal = normals[i]
                };
            }
            var groups = vertInfo.GroupBy(x => x.vert);
            VertInfo[] processedVertInfo = new VertInfo[vertInfo.Length];
            int index = 0;
            foreach (IGrouping<Vector3, VertInfo> group in groups) {
                Vector3 avgNormal = zeroVec;
                foreach (VertInfo item in group) {
                    avgNormal += item.normal;
                }
                avgNormal = avgNormal / group.Count();
                foreach (VertInfo item in group) {
                    processedVertInfo[index] = new VertInfo() {
                        vert = item.vert,
                        origIndex = item.origIndex,
                        normal = item.normal,
                        averagedNormal = avgNormal
                    };
                    index++;
                }
            }
            Color[] colors = new Color[verts.Length];
            for (int i = 0; i < processedVertInfo.Length; i++) {
                VertInfo info = processedVertInfo[i];

                int origIndex = info.origIndex;
                Vector3 normal = info.averagedNormal;
                Color normColor = new Color(normal.x, normal.y, normal.z, 1);
                colors[origIndex] = normColor;
            }
            meshSource.mesh.colors = colors;
        }
    }

    private struct VertInfo {
        public Vector3 vert;
        public int origIndex;
        public Vector3 normal;
        public Vector3 averagedNormal;
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
