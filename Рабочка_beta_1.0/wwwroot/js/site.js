document.addEventListener("DOMContentLoaded", () => {

    const input = document.getElementById("search-input");
    const container = document.getElementById("jobsContainer");
    const categoryLinks = document.querySelectorAll(".category-link");
    const searchButton = document.querySelector(".menu-container__search-job-btn");

    let selectedCategory = "0";
    
   
    function updateJobs() {

        const query = encodeURIComponent(input.value);
        fetch(`/Home/Search?query=${query}&category=${selectedCategory}`)
            .then(response => response.json())
            .then(data => {
                container.innerHTML = "";
                if (data.length === 0) {
                    container.innerHTML = "<p>Ничего не нашлось</p>";
                    return;
                }
                data.forEach(job => {
                    let formattedSalary = "Зарплата не указана";
                    if (job.salary !== undefined && job.salary !== null && job.salary > 0) {
                        formattedSalary = job.salary.toLocaleString() + " ₽";
                    }
                    const date = job.createdAt.split('T')[0].split('-').reverse().join('.');
                    container.innerHTML += `
                     <a href="/DetailInfo/Details/${job.id}" class="job-container__card-link">  
                        <div class="jobs-container__card">
                            <div class="jobs-container__card-info">
                                <div class="jobs-container__card-title">${job.title}</div>
                                <div class="jobs-container__card-employer">${job.employer?.username || 'Не указан'}</div>
                                <div class="jobs-container__card-price">
                                    <p class="jobs-container__card-price-p1">${formattedSalary}</p>
                                </div>
                                <div class="jobs-container__card-description">
                                    <p class="jobs-container__card-description-text">${job.description}</p>
                                </div>
                                <div class="jobs-container__card-footer">
                                    <div class="jobs-container__card-info-footer">
                                        <div class="jobs-container__card-location">
                                            <span class="jobs-container__card-location-text">${job.location}</span>
                                        </div>
                                        <div class="jobs-container__card-date">
                                            <span class="jobs-container__card-date-text">${date}</span>
                                        </div>
                                        <div class="jobs-container__card-views">
                                            <span class="jobs-container__card-views-text">${job.views}</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                     </a>
                    `;
                });

            });
    }

    // поиск
    searchButton.addEventListener("click", (e) => {
        e.preventDefault();
        updateJobs()
    })

    // категории
    categoryLinks.forEach(link => {

        link.addEventListener("click", e => {

            e.preventDefault();

            categoryLinks.forEach(l => l.classList.remove("active"));

            link.classList.add("active");

            selectedCategory = link.dataset.id || "0";

            updateJobs();

        });

    });

});