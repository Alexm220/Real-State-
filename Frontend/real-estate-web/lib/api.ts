import axios from 'axios';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7001/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Property interfaces
export interface PropertyDto {
  id: string;
  idOwner: string;
  name: string;
  address: string;
  price: number;
  image?: string;
  codeInternal: string;
  year: number;
  owner?: OwnerDto;
}

export interface PropertyDetailDto extends PropertyDto {
  images: PropertyImageDto[];
  traces: PropertyTraceDto[];
}

export interface PropertyImageDto {
  id: string;
  file: string;
  enabled: boolean;
}

export interface PropertyTraceDto {
  id: string;
  dateSale: string;
  name: string;
  value: number;
  tax: number;
}

export interface OwnerDto {
  id: string;
  idOwner: string;
  name: string;
  address: string;
  photo?: string;
  birthday: string;
}

export interface PropertyFilterDto {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
}

export interface PagedResultDto<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

// API functions
export const propertyApi = {
  getProperties: async (filter: PropertyFilterDto): Promise<PagedResultDto<PropertyDto>> => {
    const response = await api.get('/properties', { params: filter });
    return response.data;
  },

  getProperty: async (id: string): Promise<PropertyDetailDto> => {
    const response = await api.get(`/properties/${id}`);
    return response.data;
  },

  createProperty: async (property: Omit<PropertyDto, 'id'>): Promise<PropertyDto> => {
    const response = await api.post('/properties', property);
    return response.data;
  },

  updateProperty: async (id: string, property: Partial<PropertyDto>): Promise<PropertyDto> => {
    const response = await api.put(`/properties/${id}`, property);
    return response.data;
  },

  deleteProperty: async (id: string): Promise<void> => {
    await api.delete(`/properties/${id}`);
  },
};

export const ownerApi = {
  getOwner: async (id: string): Promise<OwnerDto> => {
    const response = await api.get(`/owners/${id}`);
    return response.data;
  },

  getOwnerByIdOwner: async (idOwner: string): Promise<OwnerDto> => {
    const response = await api.get(`/owners/by-id-owner/${idOwner}`);
    return response.data;
  },

  createOwner: async (owner: Omit<OwnerDto, 'id'>): Promise<OwnerDto> => {
    const response = await api.post('/owners', owner);
    return response.data;
  },

  updateOwner: async (id: string, owner: Partial<OwnerDto>): Promise<OwnerDto> => {
    const response = await api.put(`/owners/${id}`, owner);
    return response.data;
  },

  deleteOwner: async (id: string): Promise<void> => {
    await api.delete(`/owners/${id}`);
  },
};

export default api;
