using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class InPurchasingApp : MonoBehaviour, IDetailedStoreListener
{
	// Identifikátory produktů
	public string cId = "50Coin",
		ncId = "removeads",
		sId = "VipPayment";
	IStoreController sController;

	public void Start()
	{
		BuilderSetup();
		OnInitialized(sController, null);
	}
	void BuilderSetup()
	{
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		builder.AddProduct(ncId, ProductType.NonConsumable);
		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		sController = controller;
		CheckForCosumeable(cId);
	}

	void CheckForCosumeable(string id)
	{
		if (sController.products.WithID(id).hasReceipt)
		{
			var product = sController.products.WithID(id);
			if (product != null && product.hasReceipt)
			{
				sController.ConfirmPendingPurchase(product);
			}
		}
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogError("Inicializace selhala: " + error);
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.LogError("Inicializace selhala: " + error + " " + message);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
	{
		Debug.LogError("Nákup produktu " + product.definition.id + " selhal: " + failureDescription.reason + " " + failureDescription.message);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.LogError("Nákup produktu " + product.definition.id + " selhal: " + failureReason);
	}

	public void BuyNonConsumable()
	{
		if (sController != null)
		{
			sController.InitiatePurchase(ncId);
		}
		else
		{
			Debug.LogError("StoreController není inicializován.");
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		var product = purchaseEvent.purchasedProduct;

		if (product.definition.id == ncId)
		{
			Debug.Log("Zakoupen ne-spotřební produkt: " + product.definition.id);
			PlayerPrefs.SetInt(PlayerPrefsKeys.HasAds, 1);
			PlayerPrefs.Save();

			SaveRemoveAdsToCloud();

			MainMenu mainMenu = FindFirstObjectByType<MainMenu>();
			if (mainMenu != null)
			{
				mainMenu.OnAdsRemoved();
			}
		}
		else
		{
			Debug.Log("Neznámý produkt zakoupen: " + product.definition.id);
		}

		return PurchaseProcessingResult.Complete;
	}

	void SaveRemoveAdsToCloud()
	{
		if (PlayGamesPlatform.Instance.IsAuthenticated())
		{
			string saveData = "remove_ads=true";

			byte[] data = System.Text.Encoding.UTF8.GetBytes(saveData);

			PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
				 "MyCustomSaveGame",
				 DataSource.ReadCacheOrNetwork,
				 ConflictResolutionStrategy.UseLongestPlaytime,
				 (status, game) =>
				 {
					 if (status == SavedGameRequestStatus.Success)
					 {
						 SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
						 PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, (saveStatus, savedGame) =>
						 {
							 if (saveStatus == SavedGameRequestStatus.Success)
							 {
								 Debug.Log("Úspěšně uloženo: Stav odstranění reklam do cloudu.");
							 }
							 else
							 {
								 Debug.LogError("Nepodařilo se uložit stav odstranění reklam.");
							 }
						 });
					 }
					 else
					 {
						 Debug.LogError("Nepodařilo se otevřít uloženou hru.");
					 }
				 });
		}
		else
		{
			Debug.LogError("Uživatel není přihlášen, nelze uložit stav odstranění reklam.");
		}
	}

	public void GetSubscription()
	{
		sController.InitiatePurchase(cId);
	}

	public void OnRectTransformRemoved()
	{
		sController.InitiatePurchase(ncId);
	}
}
