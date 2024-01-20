document.addEventListener('DOMContentLoaded', function() {
    // Чек-лист документов
    const checklistItems = [
        'Паспорт или временное удостоверение личности',
        'Номер страхового свидетельства государственного пенсионного страхования (СНИЛС)',
        'Свидетельство о постановке на учет физического лица в налоговом органе (ИНН)',
        'Военный билет для военнообязанных',
        'Диплом либо студенческий билет, в их отсутствии справка из вуза',
        'Свидетельство о заключении/расторжении браке (при наличии)',
        'Свидетельство о рождении детей (при наличии)',
        'Трудовая книжка/сведения о трудовой деятельности работника (СТД-Р)',
        'Справка о доходах за 2 предыдущих года по форме 182-Н для расчета больничных листов'
    ];

    const checklist = document.querySelector('#document-checklist ul');
    const savedChecks = JSON.parse(localStorage.getItem('checkedDocuments')) || [];

    checklistItems.forEach((item, index) => {
        const li = document.createElement('li');
        li.textContent = item;

        // Восстанавливаем состояние из localStorage
        if (savedChecks.includes(index)) {
            li.classList.add('checked');
        }

        li.addEventListener('click', function() {
            this.classList.toggle('checked');
            updateLocalStorage();
        });

        checklist.appendChild(li);
    });

    function updateLocalStorage() {
        const checkedItems = document.querySelectorAll('#document-checklist ul li.checked');
        const checkedIndexes = Array.from(checkedItems).map(item => checklistItems.indexOf(item.textContent));
        localStorage.setItem('checkedDocuments', JSON.stringify(checkedIndexes));
    }


    // Вопросы FAQ
    const faqItems = document.querySelectorAll('.faq-item h3');
    faqItems.forEach(item => {
        item.addEventListener('click', function() {
            this.classList.toggle('active');
            const answer = this.nextElementSibling;

            if (answer.style.maxHeight && answer.style.maxHeight !== "0px") {
                answer.style.maxHeight = "0px";
            } else {
                answer.style.maxHeight = answer.scrollHeight + "px";
            } 
        });
    });

    // Кнопка печати страницы
    document.getElementById('print-page').addEventListener('click', function() {
        window.print();
    });
});

// Дополнительные стили для чек-листа и FAQ
document.styleSheets[0].insertRule('.checked { text-decoration: line-through; }', 0);
document.styleSheets[0].insertRule('.show { display: block; }', 0);

async function initMap() {
    await ymaps3.ready;
    const {YMap, YMapDefaultSchemeLayer, YMapDefaultFeaturesLayer, YMapMarker} = ymaps3;

    // Инициализация карты
    const map = new YMap(
        document.getElementById('map'),
        {
            location: {
                center: [37.620021, 55.728144],
                zoom: 16
            }
        },
        [
            new YMapDefaultSchemeLayer(),
            new YMapDefaultFeaturesLayer()
        ]
    );

    // Создание метки
    const markerElement = document.createElement('img');
    markerElement.src = 'https://yastatic.net/s3/front-maps-static/maps-front-jsapi-3/examples/images/marker-custom-icon/yellow-capybara.png';
    markerElement.style.width = '75px';  // Размеры иконки
    markerElement.style.height = '75px';

    // Координаты, где будет размещена метка
    const markerCoordinates = [37.620021, 55.728144];

    // Добавление метки на карту
    const marker = new YMapMarker({coordinates: markerCoordinates}, markerElement);
    map.addChild(marker);
}

initMap();

