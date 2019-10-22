import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
import { products } from '../products';
let ProductListComponent = class ProductListComponent {
    constructor() {
        this.products = products;
    }
    share() {
        window.alert('The product has been shared!');
    }
};
ProductListComponent = tslib_1.__decorate([
    Component({
        selector: 'app-product-list',
        templateUrl: './product-list.component.html',
        styleUrls: ['./product-list.component.css']
    })
], ProductListComponent);
export { ProductListComponent };
/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/ 
//# sourceMappingURL=product-list.component.js.map