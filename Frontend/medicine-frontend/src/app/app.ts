import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  title = 'MediConnect';
  cartItemCount = 0;
  searchQuery = '';
  locationQuery = '';
  selectedCategory = '';
  selectedPriceRange = '';
  selectedSortBy = 'relevance';
  loading = false;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  ngOnInit() {
    // Initialize cart count from localStorage if available (only in browser)
    if (isPlatformBrowser(this.platformId)) {
      const savedCart = localStorage.getItem('medicineCart');
      if (savedCart) {
        try {
          const cart = JSON.parse(savedCart);
          this.cartItemCount = cart.totalItems || 0;
        } catch (error) {
          console.error('Error parsing saved cart:', error);
        }
      }
    }
  }

  onSearch() {
    if (this.searchQuery.trim()) {
      this.loading = true;
      // Simulate API call
      setTimeout(() => {
        console.log('Searching for:', this.searchQuery);
        this.loading = false;
      }, 1000);
    }
  }

  onCategoryChange(category: string) {
    this.selectedCategory = category;
    console.log('Category changed to:', category);
  }

  addToCart() {
    this.cartItemCount++;
    // Update localStorage
    const cart = {
      totalItems: this.cartItemCount,
      items: []
    };
    localStorage.setItem('medicineCart', JSON.stringify(cart));
  }
}
