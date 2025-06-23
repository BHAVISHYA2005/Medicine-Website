export interface Medicine {
  id: number;
  name: string;
  genericName: string;
  manufacturer: string;
  description: string;
  strength: string;
  dosageForm: string;
  activeIngredients: string;
  drugClass: string;
  isPrescriptionRequired: boolean;
  imageUrl: string;
  price?: number;
  discountPrice?: number;
  isAvailable: boolean;
  availablePharmacies: PharmacyMedicine[];
}

export interface PharmacyMedicine {
  pharmacyId: number;
  pharmacyName: string;
  pharmacyAddress: string;
  pharmacyPhone: string;
  price: number;
  discountPrice?: number;
  stockQuantity: number;
  isAvailable: boolean;
  distance: number;
  isOpen: boolean;
}

export interface MedicineSearchRequest {
  searchTerm: string;
  location?: string;
  latitude?: number;
  longitude?: number;
  maxDistance?: number;
  prescriptionRequired?: boolean;
  drugClass?: string;
  dosageForm?: string;
  minPrice?: number;
  maxPrice?: number;
  pageNumber: number;
  pageSize: number;
  sortBy: string;
  sortOrder: string;
}
