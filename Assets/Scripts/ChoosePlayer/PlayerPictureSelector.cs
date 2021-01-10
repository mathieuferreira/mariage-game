using System;
using UnityEngine;
using UnityEngine.UI;

namespace ChoosePlayer
{
    public class PlayerPictureSelector : MonoBehaviour
    {
        private const float SelectedTimerMAX = .3f;
    
        public event EventHandler OnPictureSelected;
    
        [SerializeField] private Color selectedColor = default;
        [SerializeField] private PlayerID player = default;

        private bool validated;
        private PlayerReadyButton playerReadyButton;
        private Picture[] pictures;
        private int pictureSelected;
        private float selectedTimer;

        private void Awake()
        {
            validated = false;
            selectedTimer = SelectedTimerMAX;
            pictureSelected = 0;
            playerReadyButton = transform.Find("PlayerReadyButton").GetComponent<PlayerReadyButton>();
            playerReadyButton.OnPlayerReady += PlayerReadyButtonOnOnPlayerReady;

            Sprite[] avatars = AvatarManager.GetInstance().GetAvatars(player);
            pictures = new Picture[avatars.Length];
            for (int i = 0; i < avatars.Length; i++)
            {
                pictures[i] = new Picture(transform.Find("ChoosePlayerPicture" + (i + 1)), avatars[i], selectedColor);
            }

            SelectPicture(0);
        }

        private void Update()
        {
            if (validated)
            {
                HandleColorTransition();
                return;
            }

            UserInput.Direction direction = UserInput.FindBestDirectionDown(player);
            if (direction == UserInput.Direction.Up && pictureSelected > 1)
            {
                SoundManager.GetInstance().Play("Change");
                SelectPicture(pictureSelected - 2);
            } 
            else if (direction == UserInput.Direction.Down && pictureSelected < 2)
            {
                SoundManager.GetInstance().Play("Change");
                SelectPicture(pictureSelected + 2);
            } 
            else if (direction == UserInput.Direction.Right && pictureSelected % 2 != 1)
            {
                SoundManager.GetInstance().Play("Change");
                SelectPicture(pictureSelected  + 1);
            }
            else if (direction == UserInput.Direction.Left && pictureSelected % 2 != 0)
            {
                SoundManager.GetInstance().Play("Change");
                SelectPicture(pictureSelected - 1);
            }
        }

        private void HandleColorTransition()
        {
            if (selectedTimer < 0)
                return;

            selectedTimer -= Time.deltaTime;
            float normalisedTimer = 1 - selectedTimer / SelectedTimerMAX;
            foreach (Picture picture in pictures)
            {
                picture.Animate(normalisedTimer);
            }
        }

        private void PlayerReadyButtonOnOnPlayerReady(object sender, EventArgs e)
        {
            AvatarManager.GetInstance().SetIndex(player, pictureSelected);
            validated = true;
        
            OnPictureSelected?.Invoke(this, EventArgs.Empty);
        }

        private void SelectPicture(int selectedCandidate)
        {
            pictures[pictureSelected].Unselect();
            pictureSelected = selectedCandidate;
            pictures[pictureSelected].Select();
        }

        public bool IsValidated()
        {
            return validated;
        }

        private class Picture
        {
            private readonly GameObject overlay;
            private readonly Transform border;
            private readonly Transform picture;
            private bool selected;
            private readonly Color selectedColor;
            private readonly Color baseColor;
            private bool activated;
        
            public Picture(Transform baseTransform, Sprite picture, Color selectedColor)
            {
                overlay = baseTransform.Find("Overlay").gameObject;
                border = baseTransform.Find("Border");
                this.picture = baseTransform.Find("Picture");
                baseColor = border.GetComponent<Image>().color;
                this.selectedColor = selectedColor;
                baseTransform.Find("Picture").GetComponent<Image>().sprite = picture;
                selected = false;
            }

            public void Select()
            {
                border.GetComponent<Image>().color = selectedColor;
                selected = true;
                overlay.SetActive(false);
            }

            public void Unselect()
            {
                border.GetComponent<Image>().color = baseColor;
                selected = false;
                overlay.SetActive(true);
            }

            public void Animate(float animationTimeNormalized)
            {
                Color origin = selected ? Color.white : baseColor;
                Color destination = selected ? selectedColor : GameColor.GREY;
            
                border.GetComponent<Image>().color = new Color(
                    origin.r - (origin.r - destination.r) * animationTimeNormalized,
                    origin.g - (origin.g - destination.g) * animationTimeNormalized,
                    origin.b - (origin.b - destination.b) * animationTimeNormalized,
                    1f
                );

                if (selected)
                {
                    float floatScale = 1.2f - .2f * animationTimeNormalized;
                    Vector3 scale = new Vector3(floatScale, floatScale, 1f);
                    border.localScale = scale;
                    picture.localScale = scale;
                }
            }
        }
    }
}
