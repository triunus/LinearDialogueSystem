/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestView : MonoBehaviour
{
    private GameSystems.PlainServices.TestFadInAndOutService TestFadInAndOutService = new();

    [SerializeField] private Image TestImage;
    [SerializeField] private TextMeshProUGUI TestText;
    [SerializeField] private SpriteRenderer TestSprite;

    private void Start()
    {
        StartCoroutine(this.Testing());
    }

    private IEnumerator Testing()
    {
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestImage, 0.5f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestImage, 1f, 1f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestImage, 0f, 1f));

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestText, 0.5f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestText, 1f, 1f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestText, 0f, 1f));

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestSprite, 0.5f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestSprite, 1f, 1f));
        yield return StartCoroutine(this.TestFadInAndOutService.FadeOperation(TestSprite, 0f, 1f));

        yield return new WaitForSeconds(0.3f);
    }
}
*/