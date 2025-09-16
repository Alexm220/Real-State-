import { useState } from 'react';
import { PropertyFilterDto } from '@/lib/api';

interface PropertyFilterProps {
  onFilterChange: (filter: PropertyFilterDto) => void;
  loading?: boolean;
}

export default function PropertyFilter({ onFilterChange, loading = false }: PropertyFilterProps) {
  const [filters, setFilters] = useState<PropertyFilterDto>({
    name: '',
    address: '',
    minPrice: undefined,
    maxPrice: undefined,
    page: 1,
    pageSize: 12,
  });

  const handleInputChange = (field: keyof PropertyFilterDto, value: string | number | undefined) => {
    const updatedFilters = {
      ...filters,
      [field]: value === '' ? undefined : value,
      page: 1, // Reset to first page when filters change
    };
    setFilters(updatedFilters);
    onFilterChange(updatedFilters);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onFilterChange(filters);
  };

  const clearFilters = () => {
    const clearedFilters: PropertyFilterDto = {
      name: '',
      address: '',
      minPrice: undefined,
      maxPrice: undefined,
      page: 1,
      pageSize: 12,
    };
    setFilters(clearedFilters);
    onFilterChange(clearedFilters);
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md mb-6">
      <h2 className="text-lg font-semibold text-gray-900 mb-4">Filter Properties</h2>
      
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-1">
              Property Name
            </label>
            <input
              type="text"
              id="name"
              value={filters.name || ''}
              onChange={(e) => handleInputChange('name', e.target.value)}
              placeholder="Search by name..."
              className="input-field"
              disabled={loading}
            />
          </div>
          
          <div>
            <label htmlFor="address" className="block text-sm font-medium text-gray-700 mb-1">
              Address
            </label>
            <input
              type="text"
              id="address"
              value={filters.address || ''}
              onChange={(e) => handleInputChange('address', e.target.value)}
              placeholder="Search by address..."
              className="input-field"
              disabled={loading}
            />
          </div>
          
          <div>
            <label htmlFor="minPrice" className="block text-sm font-medium text-gray-700 mb-1">
              Min Price
            </label>
            <input
              type="number"
              id="minPrice"
              value={filters.minPrice || ''}
              onChange={(e) => handleInputChange('minPrice', e.target.value ? Number(e.target.value) : undefined)}
              placeholder="$0"
              className="input-field"
              disabled={loading}
            />
          </div>
          
          <div>
            <label htmlFor="maxPrice" className="block text-sm font-medium text-gray-700 mb-1">
              Max Price
            </label>
            <input
              type="number"
              id="maxPrice"
              value={filters.maxPrice || ''}
              onChange={(e) => handleInputChange('maxPrice', e.target.value ? Number(e.target.value) : undefined)}
              placeholder="$1,000,000"
              className="input-field"
              disabled={loading}
            />
          </div>
        </div>
        
        <div className="flex gap-3 pt-4">
          <button
            type="submit"
            className="btn-primary"
            disabled={loading}
          >
            {loading ? 'Searching...' : 'Search Properties'}
          </button>
          <button
            type="button"
            onClick={clearFilters}
            className="btn-secondary"
            disabled={loading}
          >
            Clear Filters
          </button>
        </div>
      </form>
    </div>
  );
}
