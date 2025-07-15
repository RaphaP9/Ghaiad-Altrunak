using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public static class GeneralUtilities
{
    private const bool DEBUG = true;

    private static readonly Vector2Int[] directions8 = new Vector2Int[]
    {
        new Vector2Int(0,  1),  // Up
        new Vector2Int(1,  1),  // Up-Right
        new Vector2Int(1,  0),  // Right
        new Vector2Int(1, -1),  // Down-Right
        new Vector2Int(0, -1),  // Down
        new Vector2Int(-1, -1),  // Down-Left
        new Vector2Int(-1,  0),  // Left
        new Vector2Int(-1,  1),  // Up-Left
    };

    #region GUIDs
    public static string GenerateGUID()
    {
        string generatedGUID = Guid.NewGuid().ToString();
        return generatedGUID;
    }

    #endregion

    #region Angles
    public static float NormalizeAngleDeg(float angleDeg)
    {
        float normalizedAngle = (angleDeg % 360f + 360f) % 360f;
        return normalizedAngle;
    }

    public static bool AngleDegIsInQuadrant(float angleDeg, int quadrant)
    {
        switch (quadrant)
        {
            case 1:
            default:
                return AngleDegIs1stQuadrant(angleDeg);
            case 2:
                return AngleDegIs2ndQuadrant(angleDeg);
            case 3:
                return AngleDegIs3rdQuadrant(angleDeg);
            case 4:
                return AngleDegIs4thQuadrant(angleDeg);
        }
    }

    public static bool AngleDegIs1stQuadrant(float angleDeg)
    {
        float normalizedAngle = NormalizeAngleDeg(angleDeg);
        bool isFirstQuadrant = normalizedAngle >= 0f && normalizedAngle <= 90f;
        return isFirstQuadrant;
    }

    public static bool AngleDegIs2ndQuadrant(float angleDeg)
    {
        float normalizedAngle = NormalizeAngleDeg(angleDeg);
        bool isSecondQuadrant = normalizedAngle >= 90f && normalizedAngle <= 180f;
        return isSecondQuadrant;
    }

    public static bool AngleDegIs3rdQuadrant(float angleDeg)
    {
        float normalizedAngle = NormalizeAngleDeg(angleDeg);
        bool isThirdQuadrant = normalizedAngle >= 180f && normalizedAngle <= 270f;
        return isThirdQuadrant;
    }

    public static bool AngleDegIs4thQuadrant(float angleDeg)
    {
        float normalizedAngle = NormalizeAngleDeg(angleDeg);
        bool isFourthQuadrant = normalizedAngle >= 270f && normalizedAngle <= 360f;
        return isFourthQuadrant;
    }
    #endregion

    #region Vectors
    public static Vector2 SupressZComponent(Vector3 vector3) => new Vector2(vector3.x, vector3.y);

    public static Vector2Int Vector2ToVector2Int(Vector2 vector2)
    {
        Vector2Int vector2Int = new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
        return vector2Int;
    }

    public static float GetVector2AngleDegrees(Vector2 vector2) => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Vector2 GetAngleDegreesVector2(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        Vector2 vector =  new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        vector.Normalize();
        return vector;
    }

    public static Vector3 Vector2ToVector3(Vector2 vector2) => new Vector3(vector2.x, vector2.y, 0f );

    public static Vector2 RotateVector2ByAngleDegrees(Vector2 vector, float angleDegrees)
    {
        float magnitude = vector.magnitude;

        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        float rotationSin = Mathf.Sin(angleRadians);
        float rotationCos = Mathf.Cos(angleRadians);

        Vector2 rotatedVector = new Vector2(vector.x * rotationCos - vector.y * rotationSin, vector.x * rotationSin + vector.y * rotationCos);
        rotatedVector.Normalize();

        Vector2 finalVector = rotatedVector * magnitude;

        return finalVector;

    }
    #endregion

    #region VectorInts

    public static Vector2 Vector2IntToVector2(Vector2Int vector2Int)
    {
        Vector2 vector2 = new Vector2(vector2Int.x, vector2Int.y);
        return vector2;
    }

    public static Vector3 Vector2IntToVector3(Vector2Int vector2) => new Vector3(vector2.x, vector2.y, 0f);

    public static bool CheckVectorIntsAreSameDirection(Vector2Int vectorA, Vector2Int vectorB)
    {
        if(vectorA == Vector2Int.zero) return false;
        if(vectorB == Vector2Int.zero) return false;

        if(!CheckVectorIntsCollinear(vectorA, vectorB)) return false;
        if(!CheckVectorIntsSameOrientation(vectorA, vectorB)) return false;

        return true;
    }

    public static bool CheckVectorIntsCollinear(Vector2Int vectorA, Vector2Int vectorB)
    {
        if (vectorA.x * vectorB.y == vectorA.y * vectorB.x) return true;
        
        return false;
    }

    public static bool CheckVectorIntsSameOrientation(Vector2Int vectorA, Vector2Int vectorB) //Same sing in X and Y
    {
        if (vectorA.x * vectorB.x + vectorA.y * vectorB.y > 0) return true;

        return false;
    }

    public static Vector2Int ClampVector2To8Direction(Vector2 vector2)
    {
        if(vector2 == Vector2Int.zero) return Vector2Int.zero;

        Vector2Int closestDirection = directions8[0];
        float maxDotProduct = Vector2.Dot(vector2, closestDirection);

        foreach(Vector2Int direction in directions8)
        {
            Vector2 floatDirection = Vector2IntToVector2(direction);
            float dot = Vector2.Dot(vector2, floatDirection.normalized);

            if (dot > maxDotProduct)
            {
                maxDotProduct = dot;
                closestDirection = direction;
            }
        }

        return closestDirection;
    }

    #endregion

    #region Floats
    public static float RoundToNDecimalPlaces(float number, int decimalPlaces) => Mathf.Round(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
    public static float FloorToNDecimalPlaces(float number, int decimalPlaces) => Mathf.FloorToInt(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);
    public static float CeilToNDecimalPlaces(float number, int decimalPlaces) => Mathf.CeilToInt(number * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);

    public static float ClampNumber01(float number) => Mathf.Clamp01(number);

    #endregion

    #region Transforms
    public static Vector2 TransformPositionVector2(Transform transform) => new Vector2(transform.position.x, transform.position.y);

    public static void RotateTransformTowardsVector2(Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public static List<Vector2> TransformPositionVector2List(List<Transform> transforms)
    {
        List<Vector2> vectorList = new List<Vector2>();

        foreach (Transform transform in transforms)
        {
            vectorList.Add(TransformPositionVector2(transform));
        }

        return vectorList;
    }

    public static List<Transform> GetTransformsByColliders(Collider2D[] colliders)
    {
        List<Transform> transforms = new List<Transform>();

        foreach (Collider2D collider in colliders)
        {
            Transform transform = GetTransformByCollider(collider);

            transforms.Add(transform);
        }

        return transforms;
    }

    public static Transform GetTransformByCollider(Collider2D collider) => collider.transform;

    public static List<Transform> DetectTransformsInMultipleRanges(List<Vector2> positions, float detectionRange, LayerMask layerMask)
    {
        HashSet<Transform> uniqueTransforms = new HashSet<Transform>();

        foreach (Vector2 position in positions)
        {
            List<Transform> detectedTransforms = DetectTransformsInRange(position, detectionRange, layerMask);

            foreach (Transform transform in detectedTransforms)
            {
                uniqueTransforms.Add(transform);
            }
        }

        List<Transform> uniqueTransformsList = uniqueTransforms.ToList();
        return uniqueTransformsList;
    }

    public static List<Transform> DetectTransformsInRange(Vector2 position, float detectionRange, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, detectionRange, layerMask);

        List<Transform> detectedTransforms = GetTransformsByColliders(colliders);

        return detectedTransforms;
    }
    #endregion

    #region Rect Transforms
    public static ScreenQuadrant GetScreenQuadrant(RectTransform rectTransform, Camera camera = null)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera,rectTransform.position);

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        bool isLeft = screenPoint.x < screenWidth/2f;
        bool isBottom = screenPoint.y < screenHeight / 2f;

        if (isBottom && isLeft) return ScreenQuadrant.BottomLeft;
        if (isBottom && !isLeft) return ScreenQuadrant.BottomRight;
        if (!isBottom && isLeft) return ScreenQuadrant.TopLeft;

        return ScreenQuadrant.TopRight;
    }

    public static void AdjustRectTransformPivotToScreenQuadrant(RectTransform rectTransform, ScreenQuadrant screenQuadrant, RectTransform pivotRectTransform)
    {
        switch (screenQuadrant)
        {
            case ScreenQuadrant.BottomLeft:
                rectTransform.pivot = new Vector2(0, 0);
                break;
            case ScreenQuadrant.BottomRight:
                rectTransform.pivot = new Vector2(1, 0);
                break;
            case ScreenQuadrant.TopLeft:
                rectTransform.pivot = new Vector2(0, 1);
                break;
            case ScreenQuadrant.TopRight:
                rectTransform.pivot = new Vector2(1, 1);
                break;
        }

        rectTransform.position = pivotRectTransform.position;
    }
    #endregion

    #region LayerMasks
    public static bool CheckGameObjectInLayerMask(GameObject gameObject, LayerMask layerMask) => ((1 << gameObject.layer) & layerMask) != 0;

    public static LayerMask CombineLayerMasks(List<LayerMask> layermasks)
    {
        int combined = 0;

        foreach (LayerMask layermask in layermasks)
        {
            combined |= layermask.value;
        }

        return combined;
    }
    #endregion

    #region Lists

    public static T ChooseRandomElementFromList<T>(List<T> elementsList) where T : class
    {
        if (elementsList.Count <= 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, elementsList.Count);
        return elementsList[randomIndex];
    }

    public static List<T> FisherYatesShuffle<T>(List<T> list)
    {
        List<T> shuffledList = list;

        System.Random random = new System.Random();

        int n = shuffledList.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (shuffledList[n], shuffledList[k]) = (shuffledList[k], shuffledList[n]);
        }

        return shuffledList;
    }

    public static List<T> AppendListsOfLists<T>(List<List<T>> listOfLists)
    {
        List<T> appendedList = new List<T>();

        foreach (List<T> list in listOfLists)
        {
            appendedList.AddRange(list);
        }

        return appendedList;
    }

    public static bool ListsAreExactlyEqual<T>(List<T> listA, List<T> listB)
    {
        return listA.SequenceEqual(listB);
    }

    public static bool ListsHaveSameContents<T>(List<T> listA, List<T> listB)
    {
        HashSet<T> setA = new HashSet<T>(listA);
        HashSet<T> setB = new HashSet<T>(listB);
        bool hasSameContents = setA.SetEquals(setB);

        return hasSameContents;
    }

    #endregion

    #region Generics

    public static bool TryGetGenericFromTransform<T>(Transform transform, out T foundGeneric) where T : class
    {
        foundGeneric = null;

        Component[] components = transform.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is T generic)
            {
                foundGeneric = generic;
                return true;
            }
        }

        return false;
    }

    public static List<T> TryGetGenericsFromTransforms<T>(List<Transform> transforms) where T : class
    {
        List<T> foundGenerics = new List<T>();

        foreach (Transform transform in transforms)
        {
            Component[] components = transform.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component is T generic)
                {
                    foundGenerics.Add(generic);
                }
            }
        }

        return foundGenerics;
    }

    public static bool TryGetGenericFromComponent<T>(Component component, out T foundGeneric) where T : class
    {
        foundGeneric = null;

        if(component is T generic)
        {
            foundGeneric = generic;
            return true;
        }

        return false;
    }

    public static List<T> TryGetGenericsFromComponents<T>(List<Component> components) where T : class
    {
        List<T> foundGenerics = new List<T>();

        foreach (Component component in components)
        {
            if (component is T generic)
            {
                foundGenerics.Add(generic);
            }
        }

        return foundGenerics;
    }
    #endregion

    #region Interfaces


    #endregion
}
