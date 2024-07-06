import * as cuid from 'cuid';

export interface BasketItem {
  id: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  brand: string;
  type: string;
}

export interface Basket {
  id: string;
  items: BasketItem[];
}

// we generate this class to set default values
export class Basket implements Basket {
  //we install cuid package to generate randuom id
  id = cuid();
  items: BasketItem[] = [];
}

export interface BasketTotals {
  shipping: number;
  subtotal: number;
  total: number;
}
