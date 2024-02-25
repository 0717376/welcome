document.addEventListener('DOMContentLoaded', function() {
    // Обработчик для заголовка чек-листа документов
    const checklistTitle = document.getElementById('document-checklist-title');
    checklistTitle.addEventListener('click', function() {
        this.classList.toggle('active');
        const checklist = this.nextElementSibling;

        if (checklist.style.maxHeight && checklist.style.maxHeight !== "0px") {
            checklist.style.maxHeight = "0px";
        } else {
            checklist.style.maxHeight = checklist.scrollHeight + "px";
        }
    });
    
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

    const map = new YMap(
        document.getElementById('map'),
        {
            location: {
                center: [37.620021, 55.728144], // Значения по умолчанию, будут обновлены
                zoom: 16
            }
        },
        [
            new YMapDefaultSchemeLayer(),
            new YMapDefaultFeaturesLayer()
        ]
    );

    const geocodeUrl = `https://geocode-maps.yandex.ru/1.x/?apikey=800741d9-0257-4522-a255-aa6608f4fe44&geocode=${encodeURIComponent(officeAddress)}&format=json`;
    console.log("Запрос к API Яндекс.Геокодера:", geocodeUrl); // Логируем запрос

    fetch(geocodeUrl)
        .then(response => response.json())
        .then(data => {
            console.log("Ответ от API Яндекс.Геокодера:", data); // Логируем весь ответ от API

            // Проверка наличия необходимых данных в ответе
            if (data.response && data.response.GeoObjectCollection.featureMember.length > 0) {
                const position = data.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos.split(' ');
                const coordinates = [parseFloat(position[1]), parseFloat(position[0])];

                const markerElement = document.createElement('img');
                markerElement.className = 'icon-marker';
                markerElement.src = '/images/logo_man.png';
                markerElement.style.width = '75px';
                markerElement.style.height = '75px';

                const marker = new YMapMarker({coordinates: coordinates}, markerElement);
                map.addChild(marker);

                map.setLocation({center: coordinates, zoom: 16});
            } else {
                console.log("Невозможно получить координаты для данного адреса");
            }
        })
        .catch(error => console.error('Ошибка при получении координат: ', error));
}

initMap();

