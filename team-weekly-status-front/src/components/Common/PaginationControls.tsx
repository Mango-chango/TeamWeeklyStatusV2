import React from 'react';
import { Pagination } from 'react-bootstrap';

interface PaginationControlsProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (pageNumber: number) => void;
}

const PaginationControls: React.FC<PaginationControlsProps> = ({
  currentPage,
  totalPages,
  onPageChange,
}) => {
  const pageRange = 5; // Number of pages to show in pagination
  const paginationItems = [];

  let startPage = Math.max(1, currentPage - Math.floor(pageRange / 2));
  let endPage = Math.min(totalPages, startPage + pageRange - 1);

  // Adjust startPage if we're near the end
  if (endPage - startPage < pageRange - 1) {
    startPage = Math.max(1, endPage - pageRange + 1);
  }

  // Previous button
  paginationItems.push(
    <Pagination.Prev
      key="prev"
      onClick={() => onPageChange(currentPage - 1)}
      disabled={currentPage === 1}
    />
  );

  // Page numbers
  for (let number = startPage; number <= endPage; number++) {
    paginationItems.push(
      <Pagination.Item
        key={number}
        active={number === currentPage}
        onClick={() => onPageChange(number)}
      >
        {number}
      </Pagination.Item>
    );
  }

  // Next button
  paginationItems.push(
    <Pagination.Next
      key="next"
      onClick={() => onPageChange(currentPage + 1)}
      disabled={currentPage === totalPages}
    />
  );

  return <Pagination>{paginationItems}</Pagination>;
};

export default PaginationControls;
