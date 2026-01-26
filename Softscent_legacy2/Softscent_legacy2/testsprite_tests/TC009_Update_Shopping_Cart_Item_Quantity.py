import asyncio
from playwright import async_api
from playwright.async_api import expect

async def run_test():
    pw = None
    browser = None
    context = None
    
    try:
        # Start a Playwright session in asynchronous mode
        pw = await async_api.async_playwright().start()
        
        # Launch a Chromium browser in headless mode with custom arguments
        browser = await pw.chromium.launch(
            headless=True,
            args=[
                "--window-size=1280,720",         # Set the browser window size
                "--disable-dev-shm-usage",        # Avoid using /dev/shm which can cause issues in containers
                "--ipc=host",                     # Use host-level IPC for better stability
                "--single-process"                # Run the browser in a single process mode
            ],
        )
        
        # Create a new browser context (like an incognito window)
        context = await browser.new_context()
        context.set_default_timeout(5000)
        
        # Open a new page in the browser context
        page = await context.new_page()
        
        # Navigate to your target URL and wait until the network request is committed
        await page.goto("http://localhost:62507", wait_until="commit", timeout=10000)
        
        # Wait for the main page to reach DOMContentLoaded state (optional for stability)
        try:
            await page.wait_for_load_state("domcontentloaded", timeout=3000)
        except async_api.Error:
            pass
        
        # Iterate through all iframes and wait for them to load as well
        for frame in page.frames:
            try:
                await frame.wait_for_load_state("domcontentloaded", timeout=3000)
            except async_api.Error:
                pass
        
        # Interact with the page elements to simulate user flow
        # -> Navigate to the products page to add product(s) to the shopping cart.
        frame = context.pages[-1]
        # Click on 'สินค้า' (Products) link to go to the products page
        elem = frame.locator('xpath=html/body/form/header/nav/div/div/ul/li[2]/a').nth(0)
        await page.wait_for_timeout(3000); await elem.click(timeout=5000)
        

        # -> Add a product to the shopping cart by clicking the first 'เพิ่มลงตะกร้า' button.
        frame = context.pages[-1]
        # Click 'เพิ่มลงตะกร้า' (Add to cart) for the first product (Peppermint Fresh)
        elem = frame.locator('xpath=html/body/form/div[2]/div/main/div/div[2]/div/div/div/div/a').nth(0)
        await page.wait_for_timeout(3000); await elem.click(timeout=5000)
        

        # -> Increase the quantity of the product by clicking the '+' button to update quantity and verify recalculation.
        frame = context.pages[-1]
        # Click '+' button to increase quantity of 'เปปเปอร์มินต์เฟรช' in cart
        elem = frame.locator('xpath=html/body/form/div[2]/div/main/div/div/div/div/button[2]').nth(0)
        await page.wait_for_timeout(3000); await elem.click(timeout=5000)
        

        # --> Assertions to verify final state
        frame = context.pages[-1]
        await expect(frame.locator('text=เปปเปอร์มินต์เฟรช').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿5.99').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=2').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿11.98').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=ยอดรวมสุทธิ').first).to_be_visible(timeout=30000)
        await expect(frame.locator('text=฿11.98').nth(1)).to_be_visible(timeout=30000)
        await asyncio.sleep(5)
    
    finally:
        if context:
            await context.close()
        if browser:
            await browser.close()
        if pw:
            await pw.stop()
            
asyncio.run(run_test())
    