using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class PlayerBinder : MonoBehaviour
{
    private PlayableDirector _director;

    private void Start()
    {
        _director = GetComponent<PlayableDirector>();
        BindPlayer();
    }

    private void BindPlayer()
    {
        PlayerController player = PlayerController.Instance;
        Animator playerAnimator = player.GetComponent<Animator>();
        TimelineAsset timelineAsset = (TimelineAsset) _director.playableAsset;
        Assert.IsNotNull(timelineAsset, "Timeline Asset Not Assigned To Binder " + name);
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track.name.Equals("PlayerTrack"))
            {
                _director.SetGenericBinding(track, playerAnimator);
            }
        }
    }
}
