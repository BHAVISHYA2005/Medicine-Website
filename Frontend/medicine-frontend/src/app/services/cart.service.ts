import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Medicine } from '../models/medicine.model';

export interface CartItem {
  medicine: Medicine;
  quantity: number;
  pharmacyId: string;
  pharmacyName: string;
  price: number;
}

export interface Cart {
  items: CartItem[];
  totalItems: number;
  totalPrice: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartSubject: BehaviorSubject<Cart>;
  private storageKey = 'medicineCart';

  constructor() {
    const savedCart = this.loadCartFromStorage();
    this.cartSubject = new BehaviorSubject<Cart>(savedCart);
  }

  public get cart$(): Observable<Cart> {
    return this.cartSubject.asObservable();
  }

  public get cartValue(): Cart {
    return this.cartSubject.value;
  }

  addToCart(medicine: Medicine, pharmacyId: string, pharmacyName: string, price: number, quantity: number = 1): void {
    const currentCart = this.cartValue;
    const existingItemIndex = currentCart.items.findIndex(
      item => item.medicine.id.toString() === medicine.id.toString() && item.pharmacyId === pharmacyId
    );

    if (existingItemIndex !== -1) {
      // Update quantity if item already exists
      currentCart.items[existingItemIndex].quantity += quantity;
    } else {
      // Add new item
      const newItem: CartItem = {
        medicine,
        quantity,
        pharmacyId,
        pharmacyName,
        price
      };
      currentCart.items.push(newItem);
    }

    this.updateCart(currentCart);
  }

  removeFromCart(medicineId: string, pharmacyId: string): void {
    const currentCart = this.cartValue;
    currentCart.items = currentCart.items.filter(
      item => !(item.medicine.id.toString() === medicineId && item.pharmacyId === pharmacyId)
    );
    this.updateCart(currentCart);
  }

  updateQuantity(medicineId: string, pharmacyId: string, quantity: number): void {
    const currentCart = this.cartValue;
    const itemIndex = currentCart.items.findIndex(
      item => item.medicine.id.toString() === medicineId && item.pharmacyId === pharmacyId
    );

    if (itemIndex !== -1) {
      if (quantity <= 0) {
        currentCart.items.splice(itemIndex, 1);
      } else {
        currentCart.items[itemIndex].quantity = quantity;
      }
      this.updateCart(currentCart);
    }
  }

  clearCart(): void {
    const emptyCart: Cart = {
      items: [],
      totalItems: 0,
      totalPrice: 0
    };
    this.updateCart(emptyCart);
  }

  getCartItemCount(): number {
    return this.cartValue.totalItems;
  }

  getCartTotal(): number {
    return this.cartValue.totalPrice;
  }

  private updateCart(cart: Cart): void {
    // Recalculate totals
    cart.totalItems = cart.items.reduce((total, item) => total + item.quantity, 0);
    cart.totalPrice = cart.items.reduce((total, item) => total + (item.price * item.quantity), 0);
    
    // Update the subject
    this.cartSubject.next(cart);
    
    // Save to localStorage
    this.saveCartToStorage(cart);
  }

  private loadCartFromStorage(): Cart {
    try {
      const saved = localStorage.getItem(this.storageKey);
      if (saved) {
        return JSON.parse(saved);
      }
    } catch (error) {
      console.error('Error loading cart from storage:', error);
    }
    
    return {
      items: [],
      totalItems: 0,
      totalPrice: 0
    };
  }

  private saveCartToStorage(cart: Cart): void {
    try {
      localStorage.setItem(this.storageKey, JSON.stringify(cart));
    } catch (error) {
      console.error('Error saving cart to storage:', error);
    }
  }
}
