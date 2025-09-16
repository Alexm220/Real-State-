import Image from 'next/image';
import Link from 'next/link';
import { PropertyDto } from '@/lib/api';

interface PropertyCardProps {
  property: PropertyDto;
}

export default function PropertyCard({ property }: PropertyCardProps) {
  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(price);
  };

  return (
    <div className="card overflow-hidden">
      <div className="relative h-48 w-full">
        <Image
          src={property.image || 'https://via.placeholder.com/400x300?text=No+Image'}
          alt={property.name}
          fill
          className="object-cover"
        />
      </div>
      <div className="p-6">
        <div className="flex justify-between items-start mb-2">
          <h3 className="text-lg font-semibold text-gray-900 truncate">
            {property.name}
          </h3>
          <span className="text-sm text-gray-500 ml-2">
            {property.year}
          </span>
        </div>
        
        <p className="text-gray-600 text-sm mb-3 line-clamp-2">
          {property.address}
        </p>
        
        <div className="flex justify-between items-center mb-4">
          <span className="text-2xl font-bold text-primary-600">
            {formatPrice(property.price)}
          </span>
          <span className="text-xs text-gray-500 bg-gray-100 px-2 py-1 rounded">
            #{property.codeInternal}
          </span>
        </div>
        
        {property.owner && (
          <div className="mb-4 p-3 bg-gray-50 rounded-lg">
            <p className="text-sm text-gray-600">
              <span className="font-medium">Owner:</span> {property.owner.name}
            </p>
          </div>
        )}
        
        <div className="flex gap-2">
          <Link
            href={`/properties/${property.id}`}
            className="flex-1 btn-primary text-center"
          >
            View Details
          </Link>
          <button className="btn-secondary">
            Edit
          </button>
        </div>
      </div>
    </div>
  );
}
