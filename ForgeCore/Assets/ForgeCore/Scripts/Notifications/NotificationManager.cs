using System;
using ForgeCore.Utility;
using UnityEngine;

namespace ForgeCore.Notifications
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [Header("Notification Prefabs")]
        [SerializeField] private Notification notificationHeadbar;
        [SerializeField] private Notification notificationSidebar;

        [Header("UI Elements")] 
        [SerializeField] private Transform sidebarContainer;

        public void Notify(string message, NotificationType notificationType)
        {
            var notification = notificationType switch
            {
                NotificationType.Headbar => Instantiate(notificationHeadbar, transform),
                NotificationType.Sidebar => Instantiate(notificationSidebar, sidebarContainer),
                _ => throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null)
            };
            
            notification.SetNotification(message);
        }
    }

    public enum NotificationType
    {
        Headbar,
        Sidebar
    }
}