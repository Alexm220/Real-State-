'use client';

import { useState, useEffect } from 'react';
import { useParams } from 'next/navigation';
import Image from 'next/image';
import Link from 'next/link';
import { propertyApi, PropertyDetailDto } from '@/lib/api';
import { ArrowLeftIcon, CalendarIcon, CurrencyDollarIcon, HomeIcon, UserIcon } from '@heroicons/react/24/outline';

export default function PropertyDetail() {
  const params = useParams();
  const [property, setProperty] = useState<PropertyDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedImageIndex, setSelectedImageIndex] = useState(0);

  useEffect(() => {
    const fetchProperty = async () => {
      if (!params.id) return;
      
      try {
        setLoading(true);
        setError(null);
        const result = await propertyApi.getProperty(params.id as string);
        setProperty(result);
      } catch (err) {
        setError('Failed to fetch property details. Please try again.');
        console.error('Error fetching property:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchProperty();
  }, [params.id]);

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(price);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  if (loading) {
    return (
      <div className="animate-pulse space-y-6">
        <div className="h-8 bg-gray-300 rounded w-1/4"></div>
        <div className="h-96 bg-gray-300 rounded-lg"></div>
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-2 space-y-4">
            <div className="h-6 bg-gray-300 rounded w-3/4"></div>
            <div className="h-4 bg-gray-300 rounded w-1/2"></div>
            <div className="h-20 bg-gray-300 rounded"></div>
          </div>
          <div className="space-y-4">
            <div className="h-32 bg-gray-300 rounded"></div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <div className="mx-auto h-24 w-24 text-red-400">
          <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
          </svg>
        </div>
        <h3 className="mt-4 text-lg font-medium text-gray-900">Error loading property</h3>
        <p className="mt-2 text-gray-500">{error}</p>
        <div className="mt-6">
          <Link href="/" className="btn-primary">
            Back to Properties
          </Link>
        </div>
      </div>
    );
  }

  if (!property) {
    return (
      <div className="text-center py-12">
        <h3 className="text-lg font-medium text-gray-900">Property not found</h3>
        <p className="mt-2 text-gray-500">The property you're looking for doesn't exist.</p>
        <div className="mt-6">
          <Link href="/" className="btn-primary">
            Back to Properties
          </Link>
        </div>
      </div>
    );
  }

  const displayImages = property.images.length > 0 ? property.images : [{ id: '1', file: 'https://via.placeholder.com/800x600?text=No+Image', enabled: true }];

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <Link href="/" className="flex items-center text-gray-600 hover:text-gray-900">
          <ArrowLeftIcon className="h-5 w-5 mr-2" />
          Back to Properties
        </Link>
      </div>

      <div className="bg-white rounded-lg shadow-lg overflow-hidden">
        {/* Image Gallery */}
        <div className="relative h-96 lg:h-[500px]">
          <Image
            src={displayImages[selectedImageIndex]?.file || 'https://via.placeholder.com/800x600?text=No+Image'}
            alt={property.name}
            fill
            className="object-cover"
          />
          {displayImages.length > 1 && (
            <div className="absolute bottom-4 left-1/2 transform -translate-x-1/2 flex gap-2">
              {displayImages.map((_, index) => (
                <button
                  key={index}
                  onClick={() => setSelectedImageIndex(index)}
                  className={`w-3 h-3 rounded-full ${
                    index === selectedImageIndex ? 'bg-white' : 'bg-white/50'
                  }`}
                />
              ))}
            </div>
          )}
        </div>

        <div className="p-8">
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Main Content */}
            <div className="lg:col-span-2 space-y-6">
              <div>
                <div className="flex justify-between items-start mb-4">
                  <h1 className="text-3xl font-bold text-gray-900">{property.name}</h1>
                  <span className="text-sm text-gray-500 bg-gray-100 px-3 py-1 rounded-full">
                    #{property.codeInternal}
                  </span>
                </div>
                
                <div className="flex items-center text-gray-600 mb-4">
                  <HomeIcon className="h-5 w-5 mr-2" />
                  {property.address}
                </div>
                
                <div className="text-4xl font-bold text-primary-600 mb-6">
                  {formatPrice(property.price)}
                </div>
              </div>

              {/* Property Details */}
              <div className="bg-gray-50 rounded-lg p-6">
                <h2 className="text-xl font-semibold text-gray-900 mb-4">Property Details</h2>
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <span className="text-gray-600">Year Built:</span>
                    <span className="ml-2 font-medium">{property.year}</span>
                  </div>
                  <div>
                    <span className="text-gray-600">Property ID:</span>
                    <span className="ml-2 font-medium">{property.codeInternal}</span>
                  </div>
                </div>
              </div>

              {/* Property Traces */}
              {property.traces && property.traces.length > 0 && (
                <div className="bg-gray-50 rounded-lg p-6">
                  <h2 className="text-xl font-semibold text-gray-900 mb-4">Transaction History</h2>
                  <div className="space-y-4">
                    {property.traces.map((trace) => (
                      <div key={trace.id} className="border-l-4 border-primary-500 pl-4">
                        <div className="flex justify-between items-start">
                          <div>
                            <h3 className="font-medium text-gray-900">{trace.name}</h3>
                            <div className="flex items-center text-sm text-gray-600 mt-1">
                              <CalendarIcon className="h-4 w-4 mr-1" />
                              {formatDate(trace.dateSale)}
                            </div>
                          </div>
                          <div className="text-right">
                            <div className="font-semibold text-gray-900">
                              {formatPrice(trace.value)}
                            </div>
                            <div className="text-sm text-gray-600">
                              Tax: {formatPrice(trace.tax)}
                            </div>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              )}
            </div>

            {/* Sidebar */}
            <div className="space-y-6">
              {/* Owner Information */}
              {property.owner && (
                <div className="bg-gray-50 rounded-lg p-6">
                  <h2 className="text-xl font-semibold text-gray-900 mb-4">Owner Information</h2>
                  <div className="space-y-3">
                    <div className="flex items-center">
                      <UserIcon className="h-5 w-5 text-gray-400 mr-3" />
                      <div>
                        <div className="font-medium text-gray-900">{property.owner.name}</div>
                        <div className="text-sm text-gray-600">ID: {property.owner.idOwner}</div>
                      </div>
                    </div>
                    <div className="flex items-start">
                      <HomeIcon className="h-5 w-5 text-gray-400 mr-3 mt-0.5" />
                      <div className="text-sm text-gray-600">{property.owner.address}</div>
                    </div>
                    <div className="flex items-center">
                      <CalendarIcon className="h-5 w-5 text-gray-400 mr-3" />
                      <div className="text-sm text-gray-600">
                        Born: {formatDate(property.owner.birthday)}
                      </div>
                    </div>
                  </div>
                </div>
              )}

              {/* Actions */}
              <div className="space-y-3">
                <button className="w-full btn-primary">
                  Contact Owner
                </button>
                <button className="w-full btn-secondary">
                  Schedule Viewing
                </button>
                <button className="w-full btn-secondary">
                  Save Property
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Thumbnail Gallery */}
      {displayImages.length > 1 && (
        <div className="bg-white rounded-lg shadow-md p-4">
          <h3 className="text-lg font-semibold text-gray-900 mb-4">Property Images</h3>
          <div className="grid grid-cols-4 md:grid-cols-6 lg:grid-cols-8 gap-2">
            {displayImages.map((image, index) => (
              <button
                key={index}
                onClick={() => setSelectedImageIndex(index)}
                className={`relative aspect-square rounded-lg overflow-hidden ${
                  index === selectedImageIndex ? 'ring-2 ring-primary-500' : ''
                }`}
              >
                <Image
                  src={image.file}
                  alt={`Property image ${index + 1}`}
                  fill
                  className="object-cover"
                />
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
