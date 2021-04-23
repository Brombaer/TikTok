using System;
using UnityEditor;
using UnityEngine;

public class CreateSpriteWindow : EditorWindow
{
    private Camera _camera;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateSpriteWindow window = (CreateSpriteWindow)EditorWindow.GetWindow(typeof(CreateSpriteWindow));
        window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Save") && _camera != null)
        {
            string path = EditorUtility.SaveFilePanel("default", "", "default", "png");

            if (!String.IsNullOrEmpty(path))
            {
                RenderTexture renderTexture = new RenderTexture(512, 512, 32);
                _camera.targetTexture = renderTexture;
                _camera.Render();
                _camera.targetTexture = null;
                Texture2D texture = new Texture2D(512, 512, UnityEngine.TextureFormat.RGBA32, false);
                RenderTexture.active = renderTexture;
                texture.ReadPixels(new Rect(0,0,512,512), 0, 0);
                texture.Apply();

                byte[] bytes = texture.EncodeToPNG();
            
                System.IO.File.WriteAllBytes(path, bytes);
                AssetDatabase.Refresh();
            }
        }
        
        _camera = (Camera)EditorGUILayout.ObjectField("camera", _camera, typeof(Camera), true);
    }
}
