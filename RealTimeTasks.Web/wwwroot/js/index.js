$(() => {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/jobsHub").build();

    connection.start();

    $("#add").on('click', () => {
        const name = $("#job-name").val();
        connection.invoke("AddJob", name);
        $("#job-name").val('');
    });

    connection.on("NewJob", job => {
        $("table").append(`<tr><td>${job.name}</td>
        <td><button data-job-id="${job.id}" class="btn btn-info incomplete">I'm doing this one!</button></td></tr>`);
    });

    $('table').on('click', '.incomplete', function () {
        const jobId = $(this).data('job-id');
        const userId = $("#user-id").val();
        connection.invoke("TakeJob", { jobId, userId });
    });

    connection.on("JobTaken", job => {
        const button = $(`[data-job-id=${job.id}]`);
        button.text(`Job taken by ${job.user.name}`);
        button.removeClass('btn-info incomplete');
        button.addClass('btn-warning job-taken');
        button.prop('disabled', true);
    });

    connection.on("MyJob", job => {
        const button = $(`[data-job-id=${job.id}]`);
        button.removeClass('btn-info incomplete');
        button.addClass('btn-success my-job');
        button.text('Im done!');
    });

    $('table').on('click', '.my-job', function () {
        const jobId = $(this).data('job-id');
        const userId = $("#user-id").val();
        connection.invoke("CompleteJob", { jobId, userId });
    });

    $('table').on('click', 'job-taken', function () {
        connection.invoke("Logout");
    })

    connection.on("JobCompleted", id => {
        const button = $(`[data-job-id=${id}]`);
        button.closest('tr').remove();
    })

})
