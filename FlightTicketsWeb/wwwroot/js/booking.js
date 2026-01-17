document.addEventListener('DOMContentLoaded', function () {
    const hotelCheckbox = document.getElementById('addHotelCheckbox');
    if (hotelCheckbox) {
        hotelCheckbox.addEventListener('change', function () {
            const hotelSection = document.getElementById('hotelSection');
            hotelSection.style.display = this.checked ? 'block' : 'none';
        });
    }
    const alertData = document.getElementById('alertData');
    if (alertData && alertData.dataset.showAlert === 'true') {
        const message = alertData.dataset.message || 'Бронирование успешно создано!';
        const code = alertData.dataset.code || '';

        alert(`${message}\nКод бронирования: ${code}`);
        const bookingForm = document.getElementById('bookingForm');
        if (bookingForm) {
            bookingForm.reset();
        }
        alertData.dataset.showAlert = 'false';
    }
});