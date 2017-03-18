#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class uGUITools : MonoBehaviour 
{
    
    [MenuItem("Tools/UI Anchors to Corners %&a", false, 10)]
	static void AnchorsToCorners(){
		foreach(Transform transform in Selection.transforms)
        {
			RectTransform t = transform as RectTransform;
			RectTransform pt = Selection.activeTransform.parent as RectTransform;

			if(t == null || pt == null) return;

            Undo.RecordObject(t, "UI Anchors to Corners");

            Vector2 rectPos  = t.GetPosition();
            Vector2 rectSize = t.GetSize();
            t.SetAnchorsPosition(rectPos, AnchorsCoordinateSystem.AsRect, false, false);
            t.SetAnchorsSize(rectSize, AnchorsCoordinateSystem.AsRect, false, false);

            EditorUtility.SetDirty(t);

            /*
			//DEPRECATED:
			Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
			                                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
			Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
			                                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

			t.anchorMin = newAnchorsMin;
			t.anchorMax = newAnchorsMax;
			t.offsetMin = t.offsetMax = new Vector2(0, 0);
            */
		}
	}

    [MenuItem("Tools/UI Corners to Anchors %&c", false, 10)]
	static void CornersToAnchors(){
		foreach(Transform transform in Selection.transforms){
			RectTransform t = transform as RectTransform;

			if(t == null) return;

            Undo.RecordObject(t, "UI Corners to Anchors");

			t.offsetMin = t.offsetMax = new Vector2(0, 0);

            EditorUtility.SetDirty(t);
		}
	}

    [MenuItem("Tools/UI Center Rect %&x", false, 10)]
	static void Center(){
		foreach(Transform transform in Selection.transforms){
			RectTransform t = transform as RectTransform;

			if(t == null) return;

            Undo.RecordObject(t, "UI Center Rect");

			t.SetPosition(new Vector2(.5f, .5f), CoordinateSystem.IgnoreAnchorsAndPivotNormalized, true);
            
            EditorUtility.SetDirty(t);
		}
	}

    [MenuItem("Tools/UI Center Anchors %&z", false, 10)]
	static void CenterWithAnchors(){
		foreach(Transform transform in Selection.transforms){
			RectTransform t = transform as RectTransform;

			if(t == null) return;

            Undo.RecordObject(t, "UI Center Anchors");

			t.SetPosition(new Vector2(.5f, .5f), CoordinateSystem.IgnoreAnchorsAndPivotNormalized, true);
			t.SetAnchorsPosition(new Vector2(.5f, .5f), AnchorsCoordinateSystem.Default, true, false);

            EditorUtility.SetDirty(t);
		}
	}
}
#endif
