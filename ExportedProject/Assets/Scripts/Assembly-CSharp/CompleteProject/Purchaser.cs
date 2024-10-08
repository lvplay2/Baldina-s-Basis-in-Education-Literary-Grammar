using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CompleteProject
{
	public class Purchaser : MonoBehaviour, IStoreListener
	{
		public string[] id_Consumables;

		public string[] id_NonConsumables;

		private static IStoreController _storeController;

		private static IExtensionProvider _storeExtensionProvider;

		private void Start()
		{
			if (_storeController == null)
			{
				_InitializePurchasing();
			}
		}

		private void _InitializePurchasing()
		{
			if (!_IsInitialized())
			{
				ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
				for (int i = 0; i < id_Consumables.Length; i++)
				{
					configurationBuilder.AddProduct(id_Consumables[i], ProductType.Consumable);
				}
				for (int j = 0; j < id_NonConsumables.Length; j++)
				{
					configurationBuilder.AddProduct(id_NonConsumables[j], ProductType.NonConsumable);
				}
				UnityPurchasing.Initialize(this, configurationBuilder);
			}
		}

		private bool _IsInitialized()
		{
			return _storeController != null && _storeExtensionProvider != null;
		}

		private void _BuyProductID(string productId)
		{
			if (!_IsInitialized())
			{
				Shop.This.text_ErrorNumber.text = "4004";
				Shop.This.go_PanelError.SetActive(true);
				return;
			}
			Product product = _storeController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				_storeController.InitiatePurchase(product);
				return;
			}
			Shop.This.text_ErrorNumber.text = "4005";
			Shop.This.go_PanelError.SetActive(true);
		}

		public void MY_BuyConsumable(int id)
		{
			_BuyProductID(id_Consumables[id]);
		}

		public void MY_BuyNonConsumable(int id)
		{
			_BuyProductID(id_NonConsumables[id]);
		}

		public void MY_BuySubscription()
		{
		}

		public void MY_RestorePurchases()
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.WindowsEditor)
			{
				Shop.This.MY_ShowError("P: 2003");
				return;
			}
			Shop.This.Goods = 0;
			Shop.This.MY_EnableAds();
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			_storeController = controller;
			_storeExtensionProvider = extensions;
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			for (int i = 0; i < id_Consumables.Length; i++)
			{
				if (string.Equals(args.purchasedProduct.definition.id, id_Consumables[i], StringComparison.Ordinal))
				{
					if (i == 0)
					{
						Shop.This.MY_DisableAds();
					}
					else
					{
						Shop.This.MY_BuyGoods(i);
					}
				}
			}
			for (int j = 0; j < id_NonConsumables.Length; j++)
			{
				if (string.Equals(args.purchasedProduct.definition.id, id_NonConsumables[j], StringComparison.Ordinal) && j == 0)
				{
					Shop.This.MY_DisableAds();
				}
			}
			Shop.This.MY_ShowError("P: 2001");
			return PurchaseProcessingResult.Complete;
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
			Shop.This.text_ErrorNumber.text = "5001";
			Shop.This.go_PanelError.SetActive(true);
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			Shop.This.text_ErrorNumber.text = "5011";
			Shop.This.go_PanelError.SetActive(true);
		}
	}
}
