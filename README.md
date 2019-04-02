Azure Storage Account
---
* Написать WebApi приложение для работы с изображениями.

* Приложение должно иметь два эндпоинта:

  * [Get по id](https://github.com/RomanGutovec/azureImageWorkerApi/blob/master/ImagesWorkerAPI/Controllers/ImagesController.cs) для скачивания изображения с применённым Blur эффектом
  * [Post](https://github.com/RomanGutovec/azureImageWorkerApi/blob/master/ImagesWorkerAPI/Controllers/ImagesController.cs) для загрузки изображения
![](https://github.com/RomanGutovec/azureImageWorkerApi/blob/master/Pictures/BlurWebJob.jpg)
* При вызове метода Post изображение сохраняется в Blob Storage, далее добавляется сообщение в очередь Queue Storage.
 [WebJob](https://github.com/RomanGutovec/azureImageWorkerWebJob) слушает очередь. При поступлении сообщения [WebJob](https://github.com/RomanGutovec/azureImageWorkerWebJob) заменяет изображение в Blob Storage его копией с применённым Blur эффектом.

* Логи должны писаться в Table Storage.
