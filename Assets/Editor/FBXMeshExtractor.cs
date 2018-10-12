using UnityEngine;
using UnityEditor;

public class FBXMeshExtractor
{

    private static string progressTitle = "Extracting meshes";
    private static string sourceExtension = ".fbx";
    private static string targetExtension = ".asset";

    [MenuItem("Assets/Extract Meshes", validate = true)]
    private static bool ExtractMeshesMenuItemValidate()
    {
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            if (!AssetDatabase.GetAssetPath(Selection.objects[i]).EndsWith(sourceExtension))
                return false;
        }
        return true;
    }

    [MenuItem("Assets/Extract Meshes")]
    private static void ExtractMeshesMenuItem()
    {
        EditorUtility.DisplayProgressBar(progressTitle, "", 0);
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            EditorUtility.DisplayProgressBar(progressTitle, Selection.objects[i].name, (float)i / (Selection.objects.Length - 1));
            ExtractMeshes(Selection.objects[i]);
        }
        EditorUtility.ClearProgressBar();
    }

    private static void ExtractMeshes(Object selectedObject)
    {
        //Create folder hierarchy
        string selectedObjectPath = AssetDatabase.GetAssetPath(selectedObject);
        string parentfolderPath = selectedObjectPath.Substring(0, selectedObjectPath.Length - (selectedObject.name.Length + 5));
        string objectFolderName = selectedObject.name;
        string objectFolderPath = parentfolderPath + "/" + objectFolderName;
        string meshFolderName = "Meshes";
        string meshFolderPath = objectFolderPath + "/" + meshFolderName;

        if (!AssetDatabase.IsValidFolder(objectFolderPath))
        {
            AssetDatabase.CreateFolder(parentfolderPath, objectFolderName);

            if (!AssetDatabase.IsValidFolder(meshFolderPath))
            {
                AssetDatabase.CreateFolder(objectFolderPath, meshFolderName);
            }
        }

        //Create meshes
        Object[] objects = AssetDatabase.LoadAllAssetsAtPath(selectedObjectPath);

        for (int i = 0; i < objects.Length; i++)
        {
            if(objects[i] is Mesh)
            {
                EditorUtility.DisplayProgressBar(progressTitle, selectedObject.name + " : " + objects[i].name, (float)i / (objects.Length - 1));

                Mesh mesh = Object.Instantiate(objects[i]) as Mesh;

                AssetDatabase.CreateAsset(mesh, meshFolderPath + "/" + objects[i].name + targetExtension);
            }
        }

        //Cleanup
        AssetDatabase.MoveAsset(selectedObjectPath, objectFolderPath + "/" + selectedObject.name + sourceExtension);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
