document.addEventListener('DOMContentLoaded', function () {
    const tempDataInput = document.getElementById('tempDataAlert');
    if (tempDataInput && tempDataInput.value) {
        try {
            const alertData = JSON.parse(tempDataInput.value);
            if (alertData.showAlert && alertData.message) {
                alert(alertData.message);
                tempDataInput.value = '';
            }
        } catch (e) {
            console.log('Ошибка парсинга TempData:', e);
        }
    }
});