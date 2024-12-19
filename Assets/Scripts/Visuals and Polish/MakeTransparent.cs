using UnityEngine;


public class MakeTransparent : MonoBehaviour
{
    [TextArea]
    public string notes = "Put this on objects you would like to turn transparent if they're blocking the view from the camera to the player. Note that the transparentMat needs to be transparent by default. ";
    [SerializeField] MeshRenderer myRenderer;
    [SerializeField] Material opaqueMat;
    [SerializeField] Material transparentMat;
    bool myTransparent = false;

    private void Start()
    {
        if (myRenderer == null)
            myRenderer = GetComponent<MeshRenderer>();
       
    }
    public void makeTransparent()
    {
        if (!myTransparent)
        {
            //Before, I just had the object start out as transparent and then I'd change the color here to something more transparent
            //I am concerned about performance, however, and the decal shadows don't work on Transparent-style materials
            myRenderer.material = transparentMat;
            myTransparent = true;
        }    
        
    }

    public void makeOpaque()
    {
        if (myTransparent)
        {
            myRenderer.material = opaqueMat;
            myTransparent = false;
        }
        
    }
}
