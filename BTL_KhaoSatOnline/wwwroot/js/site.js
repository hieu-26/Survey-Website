// Hiển thị thông báo từ TempData
document.addEventListener('DOMContentLoaded', function () {
    // Xử lý tooltip
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Xử lý confirm trước khi xóa
    document.querySelectorAll('.delete-confirm').forEach(button => {
        button.addEventListener('click', (e) => {
            if (!confirm('Bạn có chắc muốn xóa?')) {
                e.preventDefault();
            }
        });
    });
});