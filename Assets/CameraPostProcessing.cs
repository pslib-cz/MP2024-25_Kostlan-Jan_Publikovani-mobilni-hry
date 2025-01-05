using UnityEngine;

[ExecuteInEditMode]
public class CameraPostProcessing : MonoBehaviour
{
   public Material postProcessMaterial; // Materiál s vaším shaderem

   void OnRenderImage(RenderTexture src, RenderTexture dest)
   {
	  if (postProcessMaterial != null)
	  {
		 // Aplikace materiálu na obraz kamery
		 Graphics.Blit(src, dest, postProcessMaterial);
	  }
	  else
	  {
		 // Pokud není materiál, vykreslí se původní obraz
		 Graphics.Blit(src, dest);
	  }
   }
}
