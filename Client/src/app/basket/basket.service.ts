import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/basket';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root',
})
export class BasketService {
  baseUrl = environment.apiUrl;

  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();

  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) {}

  getBasket(id: string) {
    return this.http.get<Basket>(this.baseUrl + 'basket?id=' + id).subscribe({
      next: (basket) => {
        this.basketSource.next(basket);
        this.caculateTotals();
      },
    });
  }

  setBasket(basket: Basket) {
    console.log('Setting basket:', basket);

    return this.http.post<Basket>(`${this.baseUrl}basket`, basket).subscribe({
      next: (basket) => {
        console.log('Basket set:', basket);
        this.basketSource.next(basket);
        this.caculateTotals();
      },

      error: (err) => {
        console.error('Error setting basket:', err);
      },
    });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find((x) => x.id === id);
    if (item) {
      item.quantity -= quantity;
      basket.items = basket.items.filter((x) => x.id == id);
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe({
      next: () => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem('basket_id');
      },
    });
  }

  addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find((x) => x.id === itemToAdd.id);
    if (item) item.quantity += quantity;
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  //this mapping like automapper in .net but it's manually here
  private mapProductItemToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 1,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
    };
  }

  private caculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const shipping = 0;
    const subtotal = basket.items.reduce((a, b) => b.price * b.quantity + a, 0);
    const total = shipping + subtotal;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }

  private isProduct(item: Product | BasketItem): item is Product {
    //you can chooes any property exsit in product and not in basketItem model(different name)
    return (item as Product).productBrand !== undefined;
  }
}
