using UnityEngine;
using UnityEngine.EventSystems;
public bool AddItem(ItemData item, int quantityToAdd)
{
    // 1. Verifica se o item é empilhável
    if (item.isStackable)
    {
        // PASSO A: Tentar encher pilhas existentes primeiro
        foreach (InventorySlot slot in inventorySlots)
        {
            // Se achou o mesmo item E ele não está cheio
            if (slot.itemData == item && slot.stackSize < item.maxStackSize)
            {
                // Calcula espaço livre neste slot
                int spaceLeft = item.maxStackSize - slot.stackSize;
                
                // Pega o que for menor: o que precisamos guardar OU o espaço livre
                int amountToAddInThisSlot = Mathf.Min(spaceLeft, quantityToAdd);

                // Atualiza o slot
                slot.AddQuantity(amountToAddInThisSlot);

                // Subtrai do total que ainda precisamos guardar
                quantityToAdd -= amountToAddInThisSlot;

                // Se já guardamos tudo, encerra a função
                if (quantityToAdd <= 0) 
                {
                    UpdateUI();
                    return true;
                }
            }
        }
    }

    // PASSO B: Se chegamos aqui, ou o item não empilha, 
    // ou as pilhas existentes encheram e ainda sobrou item (quantityToAdd > 0).
    // Precisamos preencher slots vazios.
    
    while (quantityToAdd > 0)
    {
        // Procura um slot vazio (null ou id vazio, dependendo da sua implementação)
        InventorySlot emptySlot = inventorySlots.Find(s => s.itemData == null);

        if (emptySlot != null)
        {
            // Quanto cabe num slot novo? (O limite da pilha ou o que sobrou)
            int amountForNewSlot = Mathf.Min(item.maxStackSize, quantityToAdd);

            // Configura o novo slot
            emptySlot.itemData = item;
            emptySlot.stackSize = amountForNewSlot;

            // Subtrai do montante
            quantityToAdd -= amountForNewSlot;
        }
        else
        {
            // Inventário CHEIO!
            // O que sobrou em 'quantityToAdd' deve ser dropado no chão ou rejeitado.
            Debug.Log($"Inventário cheio! Sobraram {quantityToAdd} itens.");
            
            // Opcional: DropRemainingItems(item, quantityToAdd);
            
            UpdateUI();
            return false; // Indica que não coube tudo
        }
    }

    UpdateUI();
    return true; // Tudo guardado com sucesso
}