using UnityEditor;
using System.IO;

public class ScriptTemplateProcessor : AssetModificationProcessor
{
    static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", ""); // 避免處理到 meta 檔
        if (path.EndsWith(".cs"))
        {
            string fullPath = Path.GetFullPath(path);

            if (File.Exists(fullPath))
            {
                // 取得檔案名稱（不含副檔名），用來替換 #SCRIPTNAME#
                string className = Path.GetFileNameWithoutExtension(fullPath);

                // 你想要的模板內容
                string template =
$@"using UnityEngine;

public class {className} : Base, IStart, IUpdate
{{
    public void OnStart()
    {{
        
    }}

    public void OnUpdate()
    {{
        
    }}
}}";

                // 覆蓋檔案
                File.WriteAllText(fullPath, template);
            }
        }

    }
}