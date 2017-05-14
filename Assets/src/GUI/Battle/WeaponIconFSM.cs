using Caveman.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Caveman.UI.Battle
{
	[RequireComponent(typeof(Image))]
	public class WeaponIconFSM : MonoBehaviour
	{
		[SerializeField] private Sprite stone;
		[SerializeField] private Sprite sword;

		private Image _image;

		public void Start()
		{
			_image = GetComponent<Image>();
		}

		public void SetImage(WeaponType type)
		{
			switch (type)
			{
				case WeaponType.Stone:
				{
					_image.sprite = stone;
					break;
				}
				case WeaponType.Sword:
				{
					_image.sprite = sword;
					break;
				}
			}
		}

	}
}
