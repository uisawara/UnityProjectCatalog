using UnityEngine;

internal static class IndentUtil
{
    private const int Indent = 4;

    public static string GetIndentedLog(Transform transform, int extraIndent = 0)
    {
        var depth = 0;
        var currentTransform = transform;
        while (currentTransform.parent != null)
        {
            depth++;
            currentTransform = currentTransform.parent;
        }

        // 親オブジェクトの階層に基づいてインデントを追加
        return new string(' ', depth * Indent + 0 /*extraIndent*/);
    }
}
