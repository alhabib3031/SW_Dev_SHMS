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