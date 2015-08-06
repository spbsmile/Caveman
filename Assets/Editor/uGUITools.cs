#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class uGUITools : MonoBehaviour 
{
    
    [MenuItem("Tools/uGui Anchors to Corners %&a")]
	static void AnchorsToCorners(){
		foreach(Transform transform in Selection.transforms){
			RectTransform t = transform as RectTransform;
			RectTransform pt = Selection.activeTransform.parent as RectTransform;

			if(t == null || pt == null) return;
			
			Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
			                                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
			Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
			                                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

			t.anchorMin = newAnchorsMin;
			t.anchorMax = newAnchorsMax;
			t.offsetMin = t.offsetMax = new Vector2(0, 0);
		}
	}

    [MenuItem("Tools/uGui Corners to Anchors %&c")]
	static void CornersToAnchors(){
		foreach(Transform transform in Selection.transforms){
			RectTransform t = transform as RectTransform;

			if(t == null) return;

			t.offsetMin = t.offsetMax = new Vector2(0, 0);
		}
	}
}
#endif
