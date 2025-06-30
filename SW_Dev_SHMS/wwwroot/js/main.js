// تبديل حقول وسيلة الدفع
const paymentForm = document.getElementById("paymentForm");
if (paymentForm) {
    paymentForm.addEventListener("change", (e) => {
        const method = paymentForm.method.value;
        document.getElementById("bankFields").classList.toggle("hidden", method !== "libyan-bank");
        document.getElementById("paypalFields").classList.toggle("hidden", method !== "paypal-crypto");
    });
}

// محاكاة الإشعارات (أمكان الربط بباك-إند حقيقي لاحقاً)
function notify(message, type = "info") {
    const div = document.createElement("div");
    div.className = `toast ${type}`;
    div.textContent = message;
    document.body.appendChild(div);
    setTimeout(() => div.remove(), 4000);
}

// أمثلة: بعد رفع الطلب أو الدفع
const applicationForm = document.getElementById("applicationForm");
if (applicationForm) {
    applicationForm.addEventListener("submit", (e) => {
        e.preventDefault();
        notify("تم إرسال طلبك بنجاح. انتظر موافقة المدير.", "success");
        applicationForm.reset();
    });
}

const pendingTable = document.getElementById("pendingTable");
if (pendingTable) {
    // بيانات تجريبية؛ استبدلها باستعلام من API
    const demo = [
        { id: 1, name: "أحمد علي", status: "معلّق" },
        { id: 2, name: "سارة محمد", status: "معلّق" }
    ];
    demo.forEach((row) => {
        const tr = document.createElement("tr");
        tr.innerHTML = `<td>${row.id}</td><td>${row.name}</td><td>${row.status}</td>
      <td><button class="btn" onclick="approve(${row.id})">قبول</button></td>`;
        pendingTable.querySelector("tbody").appendChild(tr);
    });
}

function approve(id) {
    notify(`تم قبول طلب رقم ${id}`, "success");
    // حدث في الباك-إند: تغيير الحالة + إشعار الطالب
}

// ======index========
// Counter animation
document.addEventListener('DOMContentLoaded', function() {
const counters = document.querySelectorAll('.counter');

const observer = new IntersectionObserver((entries) => {
entries.forEach(entry => {
if (entry.isIntersecting) {
const counter = entry.target;
const target = parseInt(counter.getAttribute('data-target'));
const increment = target / 200;
let current = 0;

const timer = setInterval(() => {
current += increment;
counter.textContent = Math.floor(current);

if (current >= target) {
counter.textContent = target;
clearInterval(timer);
}
}, 10);

observer.unobserve(counter);
}
});
});

counters.forEach(counter => {
observer.observe(counter);
});
});

document.querySelectorAll('.counter').forEach(counter => {
    const updateCount = () => {
        const target = +counter.getAttribute('data-target');
        const count = +counter.innerText;
        const increment = target / 100;

        if (count < target) {
            counter.innerText = Math.ceil(count + increment);
            setTimeout(updateCount, 20);
        } else {
            counter.innerText = target;
        }
    };

    updateCount();
});

document.addEventListener('DOMContentLoaded', () => {
    // Counter animation for statistics
    const counters = document.querySelectorAll('.counter');
    const speed = 200; // The lower the speed, the faster the count

    counters.forEach(counter => {
        const updateCount = () => {
            const target = +counter.getAttribute('data-target');
            const count = +counter.innerText;

            const increment = target / speed;

            if (count < target) {
                counter.innerText = Math.ceil(count + increment);
                setTimeout(updateCount, 1);
            } else {
                counter.innerText = target;
            }
        };

        // Trigger count animation when element is in view
        const observer = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    updateCount();
                    observer.unobserve(entry.target); // Stop observing once animated
                }
            });
        }, { threshold: 0.5 }); // Trigger when 50% of the element is visible

        observer.observe(counter);
    });
});