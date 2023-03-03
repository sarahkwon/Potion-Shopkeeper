using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class imageLoader : MonoBehaviour
{
    [Header("Add all the images to the canvas and then add them to this array in the order they should appear")]
    public Image[] imagesToLoad;
    public float holdImgTime = 2f;
    public float fadeInTime = .2f;
    public float fadeOutTime = .2f;
    public float waitToLoadImgTime = .5f;
    public bool isCredits = false;
    public bool quitOnEndOfCredits;
    public float finalImageHold = 15f;
    public float customCreditsHoldTime = 5f;
    public float applicationHoldTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if (isCredits)
        {
            StartCoroutine(fadeLogosCredits());
        }
        //holdTime = (totalLoadTime - (3f * (fadeInTime+fadeOutTime))) / imagesToLoad.Length;
        else
        {
            StartCoroutine(fadeLogos());
        }
       
    }

    public IEnumerator fadeLogosCredits()
    {
        yield return new WaitForSeconds(waitToLoadImgTime);
        int imageNum = 0;
        Image img;
        Color imgColor;

        for (int i = 0; i < imagesToLoad.Length - 2; i++)
        {
            img = imagesToLoad[i];
            Debug.Log("Loading Correctly");
            imgColor = img.color;
            //set alpha to 0
            img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0f);
            //fade in
            for (float j = 0; j <= 1.1f; j += (Time.deltaTime / fadeInTime))
            {
                img.color = new Color(imgColor.r, imgColor.g, imgColor.b, j);
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                   
                    break;
                }
#else
                    if(Input.anyKey){
                     
                       break;

                    }
#endif
                yield return new WaitForEndOfFrame();
            }
            if(i == 1)
            {
                for (float t = 0; t < holdImgTime; t += Time.deltaTime)
                {
#if ENABLE_INPUT_SYSTEM
                    if (Keyboard.current.anyKey.wasPressedThisFrame)
                    {
                      
                        t += 100f;
                    }
#else
                    if(Input.anyKey){
                     
                        t+= 100f;

                    }
#endif

                    yield return new WaitForEndOfFrame();
                }

                
            }
            else
            {
                for (float t = 0; t < customCreditsHoldTime; t += Time.deltaTime)
                {
#if ENABLE_INPUT_SYSTEM
                    if (Keyboard.current.anyKey.wasPressedThisFrame)
                    {
                        break;
                        t += 100f;
                    }
#else
                    if(Input.anyKey){
                     break;
                        t+= 100f;

                    }
#endif

                    yield return new WaitForEndOfFrame();
                }
            }
           
            for (float k = 1f; k > -.1f; k -= (Time.deltaTime / fadeOutTime))
            {
                img.color = new Color(imgColor.r, imgColor.g, imgColor.b, k);
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    break;
                }
#else
                    if(Input.anyKey){
                        break;

                    }
#endif

                yield return new WaitForEndOfFrame();
            }
            for (float t = 0; t < waitToLoadImgTime; t += Time.deltaTime)
            {
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    t += 100f;
                }
#else
                    if(Input.anyKey){
                        t+= 100f;

                    }
#endif

                yield return new WaitForEndOfFrame();
            }
            imagesToLoad[i].enabled = false;

        }
        //fade last 2

        img = imagesToLoad[2];
        Image nextImage = imagesToLoad[3];
        //images fade into eachother
        imgColor = img.color;
        Color nextImageColor = nextImage.color;

        //set alpha to 0
        img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0f);
        nextImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0f);

        //fade in 3rd screen
        for (float j = 0; j <= 1.1f; j += (Time.deltaTime / fadeInTime))
        {
            img.color = new Color(imgColor.r, imgColor.g, imgColor.b, j);
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                break;
            }
#else
                    if(Input.anyKey){
                        break;

                    }
#endif
            yield return new WaitForEndOfFrame();
        }
        for (float t = 0; t < applicationHoldTime; t += Time.deltaTime)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                t += 100f;
            }
#else
                    if(Input.anyKey){
                        t+= 100f;

                    }
#endif

            yield return new WaitForEndOfFrame();
        }
        //fade out 3rd image, while fading in the 4th one

        for (float k = 1f; k > -.1f; k -= (Time.deltaTime / fadeOutTime))
        {
            img.color = new Color(imgColor.r, imgColor.g, imgColor.b, k);
            nextImage.color = new Color(nextImageColor.r, nextImageColor.g, nextImageColor.b, 1 - k);
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                img.enabled = false;
                break;
            }
#else
                    if(Input.anyKey){
                img.enabled = false;
                       break;

                    }
#endif
            yield return new WaitForEndOfFrame();
        }
        for (float t = 0; t < finalImageHold; t += Time.deltaTime)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                break;
            }
#else
                    if(Input.anyKey){
                      break;

                    }
#endif

            yield return new WaitForEndOfFrame();
        }
        //fade out 4th image
        for (float k = 1f; k > -.1f; k -= (Time.deltaTime / fadeOutTime))
        {           
            nextImage.color = new Color(nextImageColor.r, nextImageColor.g, nextImageColor.b, k);
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                break;
            }
#else
                    if(Input.anyKey){
                      break;

                    }
#endif
            yield return new WaitForEndOfFrame();
        }
        for (float t = 0; t < waitToLoadImgTime; t += Time.deltaTime)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                break;
                t += 100f;
            }
#else
                    if(Input.anyKey){
                break;
                       

                    }
#endif

            yield return new WaitForEndOfFrame();
        }
        nextImage.enabled = false;
        if (quitOnEndOfCredits)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
       
        //loads the next scene (Should be 1)

    }



    
    public IEnumerator fadeLogos()
    {
        yield return new WaitForSeconds(waitToLoadImgTime);
       
        for (int i = 0; i < imagesToLoad.Length; i++)
        {
            Image img = imagesToLoad[i];
           
                Debug.Log("Loading Correctly");
                Color imgColor = img.color;
                //set alpha to 0
                img.color = new Color(imgColor.r, imgColor.g, imgColor.b, 0f);
                //fade in
                for (float j = 0; j <= 1.1f; j += (Time.deltaTime / fadeInTime))
                {
                    img.color = new Color(imgColor.r, imgColor.g, imgColor.b, j);
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    
                    break; 
                }
#else
                    if(Input.anyKey){
                     
                       break;

                    }
#endif
                yield return new WaitForEndOfFrame();
                }
              for (float t = 0; t < holdImgTime; t += Time.deltaTime)
            {
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                   
                    t += 100f;
                }
#else
                    if(Input.anyKey){
                 
                        t+= 100f;

                    }
#endif

                yield return new WaitForEndOfFrame();
            }
            for (float k = 1f; k > -.1f; k -= (Time.deltaTime / fadeOutTime))
                {
                    img.color = new Color(imgColor.r, imgColor.g, imgColor.b, k);
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                   
                    break;
                }
#else
                    if(Input.anyKey){
                   
                       break;

                    }
#endif
                yield return new WaitForEndOfFrame();
                }
            for (float t = 0; t < waitToLoadImgTime; t += Time.deltaTime)
            {
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    break;
                    t += 100f;
                }
#else
                    if(Input.anyKey){
                     break;
                        t+= 100f;

                    }
#endif

                yield return new WaitForEndOfFrame();
            }
            imagesToLoad[i].enabled = false;

        }

      
        SceneManager.LoadScene(1);
       
        //loads the next scene (Should be 1)





    }
}
