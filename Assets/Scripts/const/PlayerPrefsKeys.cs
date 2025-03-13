/// <summary>
/// Konstatni týkající se PlayerPrefs. (Slouží hlavně pro zamezení chyb, když natvrdo definujeme stringy ve třídách).
/// </summary>
public class PlayerPrefsKeys
{
	// Hlasitost
	public const string MusicVolume = "MusicVolume";
	public const string SFXVolume = "SFXVolume";

	// Obrazovka a rozlišení
	public const string Resolution = "Resolution";
	public const string Quality = "Quality";
	public const string FullScreen = "FullScreen";

	// Jakékoli další nastavení, které budete potřebovat
	public const string Language = "Language";
	public const string Sensitivity = "Sensitivity";

	// Poslední uložená scéna.
	public const string LastScene = "LastScene";

	// Pokud hráč zapl hru. Playerfrefs nepodporují boolean, takže místo toho máme int.
	public const string HasPlayedBefore = "HasPlayerBefore";

	public const string HasAds = "HasAds";

	public const string LastCompletedScene = "LastCompletedScene";
}