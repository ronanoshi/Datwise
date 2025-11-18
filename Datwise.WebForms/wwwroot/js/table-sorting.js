// Table Sorting Functionality
(function() {
    // Get current sort parameter from URL
    const urlParams = new URLSearchParams(window.location.search);
    let currentSort = urlParams.get('sort') || '';

    // Add click handlers to all sortable headers
    document.querySelectorAll('[data-sortable]').forEach(th => {
        th.style.cursor = 'pointer';
        th.addEventListener('click', function() {
            const field = this.getAttribute('data-sortable');
            if (!field) return;

            // Determine sort direction
            let newSort = field;
            if (currentSort === field) {
                // Toggle to descending
                newSort = '-' + field;
            } else if (currentSort === '-' + field) {
                // Toggle back to ascending
                newSort = field;
            }
            // else: first click, sort ascending

            // Update the URL
            urlParams.set('sort', newSort);
            window.location.search = urlParams.toString();
        });

        // Add visual indicator for current sort
        updateSortIndicator(th, field);
    });

    function updateSortIndicator(th, field) {
        const content = th.textContent.trim();
        
        if (currentSort === field) {
            th.textContent = content + ' ?';
            th.style.fontWeight = 'bold';
        } else if (currentSort === '-' + field) {
            th.textContent = content + ' ?';
            th.style.fontWeight = 'bold';
        }
    }
})();
