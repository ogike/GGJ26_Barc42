using UnityEngine;
using UnityEngine.UI;

public static class CustomTweenExtensions
{
    public static LTDescr LeanShake(this RectTransform transform, float magnitude, float time)
    {
        return LeanShake(transform, new Vector3(1, 1, 1), magnitude, time);
    }
    
    public static LTDescr LeanShake(this RectTransform transform, Vector3 axis, float magnitude, float time)
    {
        Vector3 origin = transform.position;
        return LeanTween.value(transform.gameObject, 0.0f, 1.0f, time)
            .setOnUpdate(progress =>
            {
                float x = origin.x + axis.x * Random.Range(-magnitude, magnitude);
                float y = origin.y + axis.y * Random.Range(-magnitude, magnitude);
                float z = origin.z + axis.z * Random.Range(-magnitude, magnitude);

                if (time * (1 - progress) < 0.05f)
                    transform.position = origin;
                else
                    transform.position = new Vector3(x, y, z);
            });
    }
    
    public static LTDescr LeanAngularShake(this RectTransform transform, float magnitude, float time)
    {
        return LeanAngularShake(transform, new Vector3(1, 1, 1), magnitude, time);
    }
    
    //TODO: does not handle gimbal lock
    public static LTDescr LeanAngularShake(this RectTransform transform, Vector3 axis, float magnitude, float time)
    {
        Vector3 origin = transform.eulerAngles;
        return LeanTween.value(transform.gameObject, 0.0f, 1.0f, time)
            .setOnUpdate(progress =>
            {
                float x = origin.x + axis.x * Random.Range(-magnitude, magnitude);
                float y = origin.y + axis.y * Random.Range(-magnitude, magnitude);
                float z = origin.z + axis.z * Random.Range(-magnitude, magnitude);

                if (time * (1 - progress) < 0.05f)
                    transform.eulerAngles = origin;
                else
                    transform.eulerAngles = new Vector3(x, y, z);
            });
    }
    
    public static LTDescr LeanAlphaImage(this Image image, float from, float to, float time)
    {
        Color originalColor = image.color;
        originalColor.a = from;
        image.color = originalColor;
        return LeanTween.value(image.gameObject, image.color.a, to, time)
            .setOnUpdate(progress =>
            {
                Color c = image.color;
                c.a = progress;
                image.color = c;
            });
    }
    
    public static LTDescr LeanAlphaImage(this Image image, float to, float time)
    {
        return LeanTween.value(image.gameObject, image.color.a, to, time)
            .setOnUpdate(progress =>
            {
                Color c = image.color;
                c.a = progress;
                image.color = c;
            });
    }
    
}
