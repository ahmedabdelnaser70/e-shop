import * as cuid from 'cuid';

export interface BasketItem {
  id: string;
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

// we generate this class to set value for id
export class Basket implements Basket {
  //we install cuid package to generate randuom id
  id = cuid();
  items: BasketItem[] = [];
}
