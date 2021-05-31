using UnityEngine;
using EzySlice;

[RequireComponent(typeof(PlayerController))]
public class ObjectSlicer : MonoBehaviour
{
    [SerializeField]
    private Material transparentMaterial;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerStay(Collider other)
    {
        // slice only when player is slicing and only certain objects
        if (!playerController.IsSlicing || other.tag != "Sliceable")
            return;

        // slicing logic
        GameObject upperHull, lowerHull;
        GameObject source = other.gameObject;
        SlicedHull hull = SliceObject(source, transparentMaterial);
        if (hull != null)
        {
            // create lower hull
            lowerHull = hull.CreateLowerHull(source, transparentMaterial);
            lowerHull.transform.parent = source.transform.parent;
            lowerHull.transform.position = source.transform.position;

            // create upper hull
            upperHull = hull.CreateUpperHull(source, transparentMaterial);
            upperHull.transform.parent = source.transform.parent;
            upperHull.transform.position = source.transform.position;

            // add rb and move upper hull to left
            upperHull.AddComponent<MeshCollider>().convex = true;
            Rigidbody upperHullRb = upperHull.AddComponent<Rigidbody>();
            float randForce = Random.Range(0, 2) == 0 ? Random.Range(7f, 15f) : Random.Range(-7f, -15f);
            upperHullRb.AddForce(Vector3.left * randForce, ForceMode.Impulse);

            // play item effect
            ParticleSystem ps = source.GetComponentInChildren<ParticleSystem>();
            ps.transform.parent = ps.transform.parent.parent;
            ps.transform.localScale = new Vector3(1f, 1f, 1f);
            ps.Play();

            // remove old mesh
            Destroy(source);
        }
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSelectionMaterial = null)
    {
        return obj.Slice(transform.position, transform.up, crossSelectionMaterial);
    }
}
