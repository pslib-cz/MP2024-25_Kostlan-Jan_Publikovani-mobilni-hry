using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class InPurchasingApp : MonoBehaviour, IDetailedStoreListener
{
	// mám tady pro jistotu i další věci které nepotřebuji při možnosti rozšířování.
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
		Debug.LogError("Initialization failed: " + error);
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.LogError("Initialization failed: " + error + " " + message);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
	{
		Debug.LogError("Purchase of product " + product.definition.id + " failed: " + failureDescription.reason + " " + failureDescription.message);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.LogError("Purchase of product " + product.definition.id + " failed: " + failureReason);
	}

	public void BuyNonConsumable()
	{
		if (sController != null)
		{
			sController.InitiatePurchase(ncId);
		}
		else
		{
			Debug.LogError("StoreController is not initialized.");
		}
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		var product = purchaseEvent.purchasedProduct;

		if (product.definition.id == ncId) // Pokud uživatel koupil "removeads"
		{
			Debug.Log("Non-consumable product purchased: " + product.definition.id);
			PlayerPrefs.SetInt(PlayerPrefsKeys.HasAds, 1);
			PlayerPrefs.Save();

			// Zavolejte metodu v MainMenu pro odstranění reklam
			MainMenu mainMenu = FindFirstObjectByType<MainMenu>();
			if (mainMenu != null)
			{
				mainMenu.OnAdsRemoved();
			}
		}
		else
		{
			Debug.Log("Unrecognized product purchased: " + product.definition.id);
		}

		return PurchaseProcessingResult.Complete;
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
