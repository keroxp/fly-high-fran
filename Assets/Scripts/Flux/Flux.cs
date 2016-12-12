using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlyHighFran {
    public interface FluxComponent {
        void OnAction(object action);
    }
    public class Flux : MonoBehaviour {
        private static Flux shared;
        public static Flux Get() {
            if (shared != null) {
                return shared;
            }
            return shared = GameObject.FindObjectOfType<Flux>();
        }
        private Flux() {} 
        private int subscriberIds = 0;
        private Dictionary<int, FluxComponent> subscribers = new Dictionary<int, FluxComponent>();
        public int subscribe (FluxComponent obj) {
            if (!subscribers.ContainsValue(obj)) {
                var id = ++subscriberIds;
                subscribers[id] = obj;
                return id;
            } else {                
                foreach (var i in subscribers) {
                    if (i.Value.Equals(obj)) {
                        return i.Key;
                    }
                }
            }            
            throw new System.Exception("InvalidState");
        }
        public bool unsubscribe(int id) {
            return subscribers.Remove(id);
        }
        public bool isSubscribed(FluxComponent obj) {
            return subscribers.ContainsValue(obj);
        }
        public void passAction(object action) {
            foreach(var e in subscribers) {
                e.Value.OnAction(action);
            }
        }
        public void unsubscirbeAll() {
            subscribers.Clear();
        }
    }
}