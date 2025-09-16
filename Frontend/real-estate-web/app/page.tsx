'use client';

import { useState, useEffect } from 'react';
import PropertyCard from '@/components/PropertyCard';
import PropertyFilter from '@/components/PropertyFilter';
import Pagination from '@/components/Pagination';
import { propertyApi, PropertyDto, PropertyFilterDto, PagedResultDto } from '@/lib/api';

export default function Home() {
  const [properties, setProperties] = useState<PagedResultDto<PropertyDto>>({
    items: [],
    totalCount: 0,
    page: 1,
    pageSize: 12,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [currentFilter, setCurrentFilter] = useState<PropertyFilterDto>({
    page: 1,
    pageSize: 12,
  });

  const fetchProperties = async (filter: PropertyFilterDto) => {
    try {
      setLoading(true);
      setError(null);
      const result = await propertyApi.getProperties(filter);
      setProperties(result);
    } catch (err) {
      setError('Failed to fetch properties. Please try again.');
      console.error('Error fetching properties:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProperties(currentFilter);
  }, []);

  const handleFilterChange = (filter: PropertyFilterDto) => {
    setCurrentFilter(filter);
    fetchProperties(filter);
  };

  const handlePageChange = (page: number) => {
    const newFilter = { ...currentFilter, page };
    setCurrentFilter(newFilter);
    fetchProperties(newFilter);
  };

  return (
    <div className="space-y-6">
      <div className="text-center">
        <h1 className="text-3xl font-bold text-gray-900 mb-2">
          Property Listings
        </h1>
        <p className="text-gray-600">
          Discover your perfect property from our extensive collection
        </p>
      </div>

      <PropertyFilter onFilterChange={handleFilterChange} loading={loading} />

      {error && (
        <div className="bg-red-50 border border-red-200 rounded-lg p-4">
          <div className="flex">
            <div className="ml-3">
              <h3 className="text-sm font-medium text-red-800">Error</h3>
              <div className="mt-2 text-sm text-red-700">
                <p>{error}</p>
              </div>
              <div className="mt-4">
                <button
                  onClick={() => fetchProperties(currentFilter)}
                  className="btn-primary text-sm"
                >
                  Try Again
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      {loading ? (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {[...Array(8)].map((_, index) => (
            <div key={index} className="card animate-pulse">
              <div className="h-48 bg-gray-300 rounded-t-lg"></div>
              <div className="p-6 space-y-3">
                <div className="h-4 bg-gray-300 rounded w-3/4"></div>
                <div className="h-3 bg-gray-300 rounded w-1/2"></div>
                <div className="h-6 bg-gray-300 rounded w-1/3"></div>
                <div className="h-8 bg-gray-300 rounded"></div>
              </div>
            </div>
          ))}
        </div>
      ) : properties.items.length > 0 ? (
        <>
          <div className="flex justify-between items-center">
            <p className="text-gray-600">
              Showing {properties.items.length} of {properties.totalCount} properties
            </p>
            <div className="text-sm text-gray-500">
              Page {properties.page} of {properties.totalPages}
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {properties.items.map((property) => (
              <PropertyCard key={property.id} property={property} />
            ))}
          </div>

          <Pagination
            currentPage={properties.page}
            totalPages={properties.totalPages}
            onPageChange={handlePageChange}
            loading={loading}
          />
        </>
      ) : (
        <div className="text-center py-12">
          <div className="mx-auto h-24 w-24 text-gray-400">
            <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-4m-5 0H9m0 0H5m0 0h2M9 21v-4a2 2 0 012-2h2a2 2 0 012 2v4M7 7h10M7 11h6" />
            </svg>
          </div>
          <h3 className="mt-4 text-lg font-medium text-gray-900">No properties found</h3>
          <p className="mt-2 text-gray-500">
            Try adjusting your search criteria or clear the filters to see all properties.
          </p>
        </div>
      )}
    </div>
  );
}
