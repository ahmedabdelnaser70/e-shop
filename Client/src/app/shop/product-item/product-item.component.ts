import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss'],
})
export class ProductItemComponent {
  @Input() productItems?: Product;

  constructor(private basketService: BasketService) {}

  addItemToBasket() {
    //this check if productItems is null or undefine: if it truthy, it proceeds to the second part.
    //  if it falsy, the method does nothing.
    this.productItems && this.basketService.addItemToBasket(this.productItems);
  }
}
