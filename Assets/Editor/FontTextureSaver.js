import System.IO; 
@MenuItem ("Assets/Save Font Texture...")
 
static function SaveFontTexture () {
	var tex = Selection.activeObject as Texture2D;
	if (tex == null) {
		EditorUtility.DisplayDialog("No texture selected", "Please select a texture", "Cancel");
		return;
	}
	if (tex.format != TextureFormat.Alpha8) {
		EditorUtility.DisplayDialog("Wrong format", "Texture must be in uncompressed Alpha8 format", "Cancel");
		return;
	}
 
	// Convert Alpha8 texture to ARGB32 texture so it can be saved as a PNG
	var texPixels = tex.GetPixels();
	var tex2 = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
	tex2.SetPixels(texPixels);
 
	// Save texture (WriteAllBytes is not used here in order to keep compatibility with Unity iPhone)
	var texBytes = tex2.EncodeToPNG();
	var fileName = EditorUtility.SaveFilePanel("Save font texture", "", "font Texture", "png");
	if (fileName.Length > 0) {
		var f : FileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
		var b : BinaryWriter = new BinaryWriter(f);
		for (var i = 0; i < texBytes.Length; i++) b.Write(texBytes[i]);
		b.Close(); 
	}
 
	DestroyImmediate(tex2);
}