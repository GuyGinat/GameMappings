using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ToolsSuite : OdinEditorWindow
    {
        
        [MenuItem("Tools/Suite")]
        private static void ShowWindow()
        {
            var window = GetWindow<ToolsSuite>();
            window.titleContent = new GUIContent("Tools Suite");
            window.Show();
        }
        
        public bool edit = false;
        private SerializedObject so;
        private SerializedObject serializedObject;
        
        protected void OnEnable()
        {
            so = new SerializedObject(this);
            
            SceneView.duringSceneGui += DuringSceneGui;
            base.OnEnable();
        }
        
        protected override void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
            base.OnDisable();
        }

        private void DuringSceneGui(SceneView obj)
        {
            if (!edit) return;
        }

        private void CreateGUI()
        {
            
        }
    }
}